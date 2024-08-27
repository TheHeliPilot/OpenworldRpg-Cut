using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using Other;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class CriticalFail
{
    public State state;
    public CriticalFailType type;
}

[Serializable]
public class DamagePair
{
    public DamageType type;
    public float percentageReduction;
}

[Serializable]
public class FightingEnemy
{
    public EnemyCharacter character;
    public List<int> health;
    public EnemyDamageStates[] damageStates;
    public int pos;
    public List<InFightState> states;
    public DamagePair[] resistances;
    public bool isStunned;
    
    public FightingEnemy(EnemyCharacter c, int pos)
    {
        this.pos = pos;
        character = c;
        health = new List<int>();
        health = c.health.ToList();
        damageStates = new []
        {
            EnemyDamageStates.normal, EnemyDamageStates.normal, EnemyDamageStates.normal, EnemyDamageStates.normal,
            EnemyDamageStates.normal
        };
        resistances = c.resistances;
        states = new List<InFightState>();
    }

    public bool IncludesBlockState()
    {
        return states.Exists(inFightState => inFightState.state.block);
    }
}

public class TurnBasedAlternativeManager : MonoBehaviour
{
    public static TurnBasedAlternativeManager turnBasedAlternativeInstance;

    public bool isInCombat;
    public bool playerTurn;

    private List<EnemyCharacter> _enemiesBuffer = new();
    public List<FightingEnemy> currentEnemies;
    public Image[] enemyImages;
    public Button actionButton;
    public Button inventoryButton;
    [SerializeField]
    private SlideInOutButton fleeButton;
    public GameObject enemyPopup;
    public GameObject inventorySubmenu;
    public SlideInOutButton endTurnButton;
    public ActionSectionScript asc;
    public AudioCue missSound;
    private bool canFlee;
    
    private InventoryItem[] _onWinItems;
    private void Awake()
    {
        if (turnBasedAlternativeInstance == null)
            turnBasedAlternativeInstance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private bool _combatTest = true;
    private void Update()
    {
        //asc.gameObject.SetActive(isInCombat);
        //WIN
        
        if(isInCombat && Input.GetKeyDown(KeyCode.K))
            LooseFight();
        
        if (isInCombat && currentEnemies.Count <= 0 && !didFlee)
        {
            TooltipSystem.HideTooltip();
            inventorySubmenu.SetActive(true);
            isInCombat = false;
            InventoryManagerScript.instance.inventoryMenu.SetActive(false);
            onWinTrigger.Triggered(this, EventArgs.Empty);
        }

        if (_combatTest != isInCombat)
        {
            _combatTest = isInCombat;
            ExecuteEvents.Execute(inventoryButton.gameObject, new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler);
            
            if(canFlee)
                fleeButton.SetFunc(isInCombat);
        }

        bool b = true;
        foreach (PartyMember p in InventoryManagerScript.instance.party.Where(p => p.canAct))
        {
            b = false;
        }

        if (b && !_enemiesAttacking)
            endTurnButton.SetFunc(true);

        //if (!canFlee)
            //fleeButton.SetFunc(canFlee);
    }

    public void DoAction(AttackAction action, int enemy, int bodyPart, int partyMember = -1, FightingEnemy overrideEnemy = null)
    {
        //Debug.Log(enemy);
        FightingEnemy sEnemy = overrideEnemy ?? currentEnemies[0];
        bool didSelect = false;

        if (overrideEnemy == null)
        {
            foreach (FightingEnemy fightingEnemy in currentEnemies.Where(fightingEnemy => fightingEnemy.pos == enemy))
            {
                didSelect = true;
                sEnemy = fightingEnemy;
                break;
            }
        }

        if (!didSelect)
        {
            Debug.LogWarning("returning");
            TooltipSystem.ShowPopoutTooltip("Not enemy selected!");
            return;
        }

        if (InventoryManagerScript.instance.party[partyMember].stamina <= 0)
        {
            TooltipSystem.ShowPopoutTooltip("Not enough Stamina!");
            return;
        }
        
        InventoryManagerScript.instance.party[partyMember].stamina -= action.staminaCost;
        if(InventoryManagerScript.instance.party[partyMember].stamina <= 0) InventoryManagerScript.instance.party[partyMember].canAct = false;
        float random = Random.value;
        if (random < InventoryManagerScript.instance.party[partyMember].criticalFailPercentage / 100f)
        {
            switch (action.criticalFail.type)
            {
                case CriticalFailType.self:
                    InventoryManagerScript.instance.party[partyMember].states.Add(new InFightState(action.criticalFail.state));
                    break;
                case CriticalFailType.randomFriend:
                    InventoryManagerScript.instance.party[Random.Range(0, InventoryManagerScript.instance.party.Count)].states.Add(new InFightState(action.criticalFail.state));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            TooltipSystem.ShowPopoutTooltip("Critical Fail!");
            asc.UpdateStatusResistance();
            UpdateEnemies();
            return;
        }
        //Debug.Log(random + " || " + (float)sEnemy?.character.hitChancePercent[bodyPart] / 100);
        if (random <= (float)sEnemy?.character.hitChancePercent[bodyPart] / 100)
        {
            int damage = action.damage;
            foreach (DamagePair sEnemyResistance in sEnemy.resistances)
            {
                if (sEnemyResistance.type == action.damageType)
                    damage -= Convert.ToInt32((float)damage * ((float)sEnemyResistance.percentageReduction / 100f));

            }
            foreach (State es in action.bonusStates)
            {
                sEnemy.states.Add(new InFightState(es));
            }
            
            if(!sEnemy.IncludesBlockState())
                sEnemy.health[bodyPart] -= damage;
            
            if (sEnemy.health[bodyPart] < 0)
            {
                sEnemy.health[bodyPart] = 0;
            }
            Debug.Log($"{sEnemy.character.enemyName} got damaged by {action.actionName} for {action.damage}, {sEnemy.health[bodyPart]}hp left!");
            GameObject g = Instantiate(enemyPopup, enemyImages[enemy].transform);
            ///
            g.GetComponent<EnemyActionPopupScript>().Setup(action.damage.ToString());
            
            MainAudioManager.instance.SpawnAudio(action.soundEffect.GetSound(), Camera.main.transform.position, action.soundEffect.volume, 100, false, false);
        }
        else
        {
            Debug.Log($"{sEnemy.character.enemyName} dodged {action.actionName}!");
            GameObject g = Instantiate(enemyPopup, enemyImages[enemy].transform);
            g.GetComponent<EnemyActionPopupScript>().Setup("Dodged!");
            MainAudioManager.instance.SpawnAudio(missSound.GetSound(), Camera.main.transform.position, missSound.volume, 100, false, false);
        }

        if (sEnemy.health[bodyPart] <= 0 && bodyPart is 0 or 1) //check dead if head or torso
        {
            currentEnemies.Remove(sEnemy);
            StartCoroutine(EnemyDead(enemy));
        }
        else
        {
            StartCoroutine(EnemyHit(enemy));
        }
        
        
    }
    private IEnumerator EnemyDead(int enemy)
    {
        enemyImages[enemy].color = Color.gray;
        yield return new WaitForSeconds(0.25f);
        enemyImages[enemy].color = Color.clear;
        yield return new WaitForSeconds(0.5f);
        asc.UpdateStatusResistance();
        UpdateEnemies();
    }

    private IEnumerator EnemyHit(int enemy)
    {
        enemyImages[enemy].color = Color.red;
        yield return new WaitForSeconds(0.05f);
        asc.UpdateStatusResistance();
        UpdateEnemies();
    }

    public TriggerScript onWinTrigger;
    public TriggerScript onLossTrigger;
    public TriggerScript onFleeTrigger;
    public int turnsToLoss;
    public void StartCombat(EnemyCharacter[] enemies, TriggerScript winTrigger, TriggerScript lossTrigger, TriggerScript fleeTrigger, bool canFlee, int staminaDrain, State[] startStates, int turnsToLoss)
    {
        if(isInCombat) return;
        didFlee = false;
        _selectedEnemy = 0;
        asc.SelectPartyMember(0);
        onWinTrigger = winTrigger;
        onLossTrigger = lossTrigger;
        this.turnsToLoss = turnsToLoss;
        
        fleeButton.overrideOff = !canFlee;
        
        foreach (PartyMember partyMember in InventoryManagerScript.instance.party)
        {
            partyMember.stamina = 20;
            partyMember.stamina -= staminaDrain;
            foreach (State startState in startStates)
            {
                partyMember.states.Add(new InFightState(startState));
            }
        }
        InventoryManagerScript.instance.inventoryMenu.SetActive(true);
        _enemiesBuffer = enemies.ToList();
        //Debug.Log("Enemybuffer: "+_enemiesBuffer.Count);
        isInCombat = true;
        actionButton.interactable = true;   // asi ani netreba
        actionButton.transform.parent.transform.parent.gameObject.GetComponent<SlideInOutButton>().SetFunc(true);
        Invoke(nameof(ClickActionButton), .1f);
        UpdateEnemies();
        StartTurn();
        asc.UpdateStatusResistance();
    }

    private bool _enemiesAttacking;

    public void LooseFight()
    {
        isInCombat = false;
        InventoryManagerScript.instance.inventoryMenu.SetActive(false);
        onLossTrigger.Triggered(this, EventArgs.Empty);
        inventorySubmenu.SetActive(true);

        Debug.Log("lost fight");
    } 
    
    public void StartTurn()
    {
        if (InventoryManagerScript.instance.party.All(member => member.isDead) || turnsToLoss == 0)
        {
            LooseFight();
        }

        turnsToLoss--;
        
        foreach (PartyMember partyMember in InventoryManagerScript.instance.party.Where(partyMember =>
                     !partyMember.isDead))
        {
            partyMember.stamina += 10;
            
            if(partyMember.stamina > 0)
                partyMember.canAct = true;
            
            if (partyMember.stamina > 20)
                partyMember.stamina = 20;

            List<InFightState> toR = new();
            foreach (InFightState partyMemberState in partyMember.states)
            {
                if (partyMemberState.roundsLeft > 0)
                {
                    int randH = Random.Range(0, partyMember.health.Length);
                    partyMember.health[randH] -= partyMemberState.state.damageDie;
                    if (partyMember.health[randH] <= 0)
                    {
                        partyMember.isDead = true;
                        partyMember.stamina = 0;
                    }

                    if (partyMemberState.state.stun)
                        partyMember.canAct = false;
                }

                partyMemberState.roundsLeft--;
                if(partyMemberState.roundsLeft < 0)
                    toR.Add(partyMemberState);
            }

            foreach (InFightState inFightState in toR)
            {
                partyMember.states.Remove(inFightState);
            }
        }

        List<FightingEnemy> toRemoveEnemy = new();
        foreach (FightingEnemy fightingEnemy in currentEnemies)
        {
            fightingEnemy.isStunned = false;
            List<InFightState> toRemove = new();
            foreach (InFightState fightingEnemyState in fightingEnemy.states)
            {
                if (fightingEnemyState.state.damageDie > 0)
                {
                    int randomBodypart = Random.Range(0, 5);
                    fightingEnemy.health[randomBodypart] -= Random.Range(0, fightingEnemyState.state.damageDie) + 1;
                    if (fightingEnemy.health[randomBodypart] <= 0 && randomBodypart is 0 or 1) //check dead if head or torso 0
                    {
                        toRemoveEnemy.Add(fightingEnemy);
                    }
                }

                if (fightingEnemyState.state.stun) fightingEnemy.isStunned = true;
                fightingEnemyState.roundsLeft--;
                if (fightingEnemyState.roundsLeft <= 0) toRemove.Add(fightingEnemyState);
            }

            foreach (InFightState state in toRemove)
            {
                fightingEnemy.states.Remove(state);
            }
        }

        foreach (FightingEnemy fightingEnemy in toRemoveEnemy)
        {
            currentEnemies.Remove(fightingEnemy);
        }
        UpdateEnemies();
        
        asc.UpdateStatusResistance();
        //nButton.SetFunc(false);
    }

    public void EndTurn()
    {
        if (_enemiesAttacking) return;
        endTurnButton.SetFunc(false);
        StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        _enemiesAttacking = true;
        endTurnButton.SetFunc(false);

        foreach (FightingEnemy fightingEnemy in currentEnemies.Where(fightingEnemy => !fightingEnemy.isStunned))
        {
            yield return new WaitForSeconds(1);
            float attack = Random.value;
            EnemyAttack action = attack < fightingEnemy.character.mainHandChance/100
                ? fightingEnemy.character.mainHandAttack
                : fightingEnemy.character.offHandAttack;
            
            //fightingEnemy.character.mainHandChance

            switch (attack) //check for destroyed arms
            {
                case > 0.5f when fightingEnemy.health[3] <= 0:
                    continue;
                case < 0.5f when fightingEnemy.health[4] <= 0:
                    continue;
            }

            int counter = 0;
            int member;
            do
            {
                member = Random.Range(0, InventoryManagerScript.instance.party.Count);
                counter++;
            } while (InventoryManagerScript.instance.party[member].isDead && counter < 50);
            
            PartyMember partyMember = InventoryManagerScript.instance.party[member];

            GameObject g;
            int chance = fightingEnemy.health[2] > 0 ? Random.Range(0, 100) : Random.Range(50, 100); //check for destroyed legs
            if (chance > action.hitChance)
            {
                g = Instantiate(enemyPopup,
                    asc.partyMemberImages[member].transform.GetChild(0).transform.GetChild(0).transform);
                g.GetComponent<EnemyActionPopupScript>().Setup("Dodged!");
                
                MainAudioManager.instance.SpawnAudio(missSound.GetSound(), Camera.main.transform.position, missSound.volume, 100, false, false);
                continue;
            }
            
            int randH = Random.Range(0, partyMember.health.Length);
            partyMember.health[randH] -= action.attack.damage;
            g = Instantiate(enemyPopup,
                asc.partyMemberImages[member].transform.GetChild(0).transform.GetChild(0).transform);
            g.GetComponent<EnemyActionPopupScript>().Setup(action.attack.damage.ToString());
            if (partyMember.health[randH] <= 0)
            {
                partyMember.isDead = true;
                partyMember.stamina = 0;
            }

            MainAudioManager.instance.SpawnAudio(action.attack.soundEffect.GetSound(), Camera.main.transform.position, action.attack.soundEffect.volume, 100, false, false);
        }

        _enemiesAttacking = false;
        endTurnButton.SetFunc(true);
        StartTurn();
    }

    private bool didFlee = false;
    public void FleeCombat()
    {
        int maxHealth = 0;
        int currHealth = 0;
        foreach (PartyMember partyMember in InventoryManagerScript.instance.party)
        {
            currHealth += partyMember.health.Sum();

            maxHealth += partyMember.maxHealth.Sum();
        }

        float maxHealthSum = turnBasedAlternativeInstance.currentEnemies.SelectMany(fightingEnemy => fightingEnemy.character.health).Aggregate<int, float>(0, (current, i) => current + i);
        float currHealthSum = turnBasedAlternativeInstance.currentEnemies.SelectMany(fightingEnemy => fightingEnemy.health).Aggregate<int, float>(0, (current, i) => current + i);

        if (UnityEngine.Random.value < (((float)currHealth / (float)maxHealth) - (currHealthSum / maxHealthSum)))
        {
            TooltipSystem.ShowPopoutTooltip("Flee unsuccessful!");
            EndTurn();
            return;
        }
        
        TooltipSystem.ShowPopoutTooltip("Flee successful!");
        
        _enemiesBuffer.Clear();
        currentEnemies.Clear();

        inventorySubmenu.SetActive(true);
        isInCombat = false;
        InventoryManagerScript.instance.inventoryMenu.SetActive(false);
        didFlee = true;
        onFleeTrigger?.Triggered(this, EventArgs.Empty);
    }
    
    private void ClickActionButton()
    {
        ExecuteEvents.Execute(actionButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }
    public void ClickInventoryButton()
    {
        //ExecuteEvents.Execute(inventoryButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        inventoryButton.onClick.Invoke();
    }
    
    public void UpdateEnemies()
    {

        for (int i = 0; i < 3; i++)
        {
            if (_enemiesBuffer.Count <= 0 || currentEnemies.Count >= 3)
            {
                break;
            }

            List<int> l = new(){ 0, 1, 2 };
            foreach (FightingEnemy fightingEnemy in currentEnemies)
            {
                l.Remove(fightingEnemy.pos);
            }
            int pos = l[0];

            currentEnemies.Add(new FightingEnemy(_enemiesBuffer[0], pos));
            _enemiesBuffer.RemoveAt(0);
        }
        
        foreach (Image enemyImage in enemyImages)
        {
            enemyImage.color = Color.clear;
            enemyImage.gameObject.GetComponentInChildren<Button>().interactable = false;
        }

        int curr;
        
        if (currentEnemies.Count > 0)
        {
            curr = currentEnemies[0].pos;
            enemyImages[curr].sprite = _selectedEnemy == curr
                ? currentEnemies[0].character.enemySpriteSelected
                : currentEnemies[0].character.enemySprite;
            enemyImages[curr].color = Color.white;
            enemyImages[curr].gameObject.GetComponentInChildren<Button>().interactable = true;
        }

        if (currentEnemies.Count > 1)
        {
            curr = currentEnemies[1].pos;
            enemyImages[curr].sprite = _selectedEnemy == curr
                ? currentEnemies[1].character.enemySpriteSelected
                : currentEnemies[1].character.enemySprite;
            enemyImages[curr].color = Color.white;
            enemyImages[curr].gameObject.GetComponentInChildren<Button>().interactable = true;
        }


        if (currentEnemies.Count > 2)
        {
            curr = currentEnemies[2].pos;
            enemyImages[curr].sprite = _selectedEnemy == curr
                ? currentEnemies[2].character.enemySpriteSelected
                : currentEnemies[2].character.enemySprite;
            enemyImages[curr].color = Color.white;
            enemyImages[curr].gameObject.GetComponentInChildren<Button>().interactable = true;
        }
    }

    private int _selectedEnemy;
    public void SelectEnemy(int enemy)
    {
        _selectedEnemy = enemy;
    }
}
