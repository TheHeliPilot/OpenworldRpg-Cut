using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Other;
using TMPro;
using UnityEngine;

public class FleeSectionScript : MonoBehaviour
{
    public TMP_Text fleeChanceText;
    public TMP_Text leaveBehindText;
    
    private void Update()
    {
        string lbt = "";

        foreach (PartyMember p in InventoryManagerScript.instance.party.Where(p => p.isDead))
        {
            lbt += p.speaker.GetName() + ", ";
        }

        leaveBehindText.text = lbt == "" ? "No one" : lbt;

        int maxHealth = 0;
        int currHealth = 0;
        foreach (PartyMember partyMember in InventoryManagerScript.instance.party)
        {
            currHealth += partyMember.health.Sum();

            maxHealth += partyMember.maxHealth.Sum();
        }
        float maxHealthSum = TurnBasedAlternativeManager.turnBasedAlternativeInstance.currentEnemies.SelectMany(fightingEnemy => fightingEnemy.character.health).Aggregate<int, float>(0, (current, i) => current + i);
        float currHealthSum = TurnBasedAlternativeManager.turnBasedAlternativeInstance.currentEnemies.SelectMany(fightingEnemy => fightingEnemy.health).Aggregate<int, float>(0, (current, i) => current + i);
        Debug.Log(currHealthSum);
        Debug.Log($"{((float)currHealth / (float)maxHealth)} - {(currHealthSum / maxHealthSum)}");
        fleeChanceText.text = Mathf.Clamp(Mathf.RoundToInt((((float)currHealth / (float)maxHealth) - (currHealthSum / maxHealthSum)) * 100), 0, 100) + "%";
    }
}
