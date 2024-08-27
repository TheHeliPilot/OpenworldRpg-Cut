using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Other;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace MainManagers
{
    // ReSharper disable InconsistentNaming

    #region CustomClasses

    [Serializable]
    public class QuestNeededItem
    {
        public InventoryItem item;
        public int amount;
    }

    [Serializable]
    public class DialogueLine
    {
        public DialogueLine()
        {
            
        }
        public DialogueLine(Speaker speaker, string text)
        {
            this.speaker = speaker;
            dialogue = text;
        }

        public string triggerEvent;
        public Speaker speaker;
        [TextArea]
        public string dialogue;
    }

    [Serializable]
    public class Conversation
    {
        public GameObject showCG;
        public string choiceText;
        //public bool needsItems;
        public List<QuestNeededItem> neededItems;
        public bool autoContinue;
    
        [SerializeField]
        private DialogueLine[] lines;
    
        public int questPart;

        public DialogueLine[] GetLines()
        {
            return lines;
        }

        public DialogueLine GetLineByIndex(int index)
        {
            return lines[index];
        }

        public int GetLength()
        {
            return lines.Length;
        }
    }

    [Serializable]
    public class ActiveQuest
    {
        public ActiveQuest(Quest q, int p)
        {
            quest = q;
            pos = p;
        }
    
        public Quest quest;
        public int pos;
    }
    #endregion
    
    public class MainDialogueManager : MonoBehaviour
    {
        public static MainDialogueManager Instance;

        public List<Quest> allQuests;

        public AudioCue writeCue;
        public AudioCue rustleCue;
        public AudioCue diceCue;
        
        public List<Quest> quests;
        public List<ActiveQuest> activeQuests;

        public bool IsTalking = false;
        public GameObject dialogueObject;
        public GameObject dialogueCharacterObject;
        public Image dialogueCharacterImage;
        public TMP_Text dialogueText;
        public TMP_Text dialogueName;

        public Speaker player;

        public List<string> currentChoices;
        public List<ChoiceQuest> currentQuests;
        public List<Quest> finishedQuests;
        public bool isChoosing;
        
        public GameObject choicesUI;
        public GameObject choicesUIContent;
        public GameObject choiceObject;
        public SpriteRenderer[] allTalking;

        public GameObject CGObject;
        
        private List<Speaker> currentlyIncluded = new List<Speaker>();
        private Quest lastQuest;

        public bool talkingToSelf;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

        }

        private void Start()
        {
            //MainAudioManager.instance.playAmbience = false;
        }

        private void Update()
        {
            if (dialogueObject.activeSelf != IsTalking)
            {
                dialogueObject.SetActive(IsTalking);
                dialogueCharacterObject.SetActive(IsTalking);
            }
            if (CGManager.IsInCG)
                dialogueObject.SetActive(false);
            
            if (choicesUI.activeSelf != isChoosing)
            {
                choicesUI.SetActive(isChoosing);
            }
        }

        private void ShowOptions()
        {
            //Debug.LogError("yy");
            foreach (SpriteRenderer s in allTalking)
            {
                s.sprite = null;
            }
            
            /*
            foreach (DialogueLine s in lastQuest.conversations[lastQuest.conversations.Count-1].GetLines())
            {
                bool canAdd = true;
                foreach (Speaker speaker in currentlyIncluded.Where(speaker => s.speaker == speaker))
                {
                    canAdd = false;
                }
                if(canAdd)
                    currentlyIncluded.Add(s.speaker);
            }

            if (currentlyIncluded.Count > 4)
            {
                Debug.LogError("MORE THEN 4 SPEAKERS IN QUEST!!");
                return;
            }

            allTalking[0].sprite = currentlyIncluded[0].GetSprite();
            if(currentlyIncluded.Count > 1)
                allTalking[1].sprite = currentlyIncluded[1].GetSprite();
            if(currentlyIncluded.Count > 2)
                allTalking[2].sprite = currentlyIncluded[2].GetSprite();
            if(currentlyIncluded.Count > 3)
                allTalking[3].sprite = currentlyIncluded[3].GetSprite();
            */
            
            foreach(Transform t in choicesUIContent.transform)
                Destroy(t.gameObject);
            
            isChoosing = true;
            for (int i = 0; i < currentChoices.Count; i++)
            {
                //Debug.Log(currentChoices[i] + " => " + i);
                GameObject g = Instantiate(choiceObject, choicesUIContent.transform);
                g.GetComponent<ChoiceButtonScript>().choice = i;
                g.GetComponent<ChoiceButtonScript>().skill = currentQuests[i].skill;
                g.GetComponentInChildren<TMP_Text>().text = currentChoices[i];
            }
        }

        public void StartChoice(int i, Skills skill)
        {
            StartCoroutine(Choice(i, skill));
        }
        
        public IEnumerator Choice(int i, Skills skill)
        {
            //Debug.Log(i);
            
            Quest q;
            isChoosing = false;
            if (currentQuests[i] == null)
            {
                Debug.LogError("Current quest null!");
                yield break;
            }

            if (currentQuests[i].minThrow > 0)
            {
                //Debug.Log("a");
                DiceRollScreenManager.active = true;

                DiceRollScreenManager.SetType(skill);
                //Debug.Log("b");

                yield return new WaitForSeconds(2f);

                MainAudioManager.instance.SpawnAudio(diceCue.GetSound(), Vector3.zero, 1, 150, false, false);

                int randThrow;
                //Debug.Log("c");

                randThrow = Random.Range(1, 21);
                DiceRollScreenManager.SetRoll(randThrow);
                yield return new WaitForSeconds(.1f);
                randThrow = Random.Range(1, 21);
                DiceRollScreenManager.SetRoll(randThrow);
                yield return new WaitForSeconds(.1f);
                randThrow = Random.Range(1, 21);
                DiceRollScreenManager.SetRoll(randThrow);
                yield return new WaitForSeconds(.1f);
                randThrow = Random.Range(1, 21);
                DiceRollScreenManager.SetRoll(randThrow);
                yield return new WaitForSeconds(.1f);
                randThrow = Random.Range(1, 21);
                DiceRollScreenManager.SetRoll(randThrow);
                yield return new WaitForSeconds(.1f);
                randThrow = Random.Range(1, 21);
                DiceRollScreenManager.SetRoll(randThrow);
                yield return new WaitForSeconds(.1f);
                randThrow = Random.Range(1, 21);
                DiceRollScreenManager.SetRoll(randThrow);
                yield return new WaitForSeconds(.1f);
                randThrow = Random.Range(1, 21);
                DiceRollScreenManager.SetRoll(randThrow);
                yield return new WaitForSeconds(2f);

                //Debug.Log("d");


                // if (currentQuests[i].minThrow <= Mathf.FloorToInt((InventoryManagerScript.instance.skills[(int)currentQuests[i].skill]-10)/2) + randThrow)
                if (currentQuests[i].minThrow <= randThrow)
                {
                    q = currentQuests[i].success;
                    DiceRollScreenManager.Finish(true);
                    //currentQuests[i].successTrigger?.Triggered(this, EventArgs.Empty);
                }
                else
                {
                    q = currentQuests[i].fail;
                    DiceRollScreenManager.Finish(false);
                    //currentQuests[i].failTrigger?.Triggered(this, EventArgs.Empty);
                }

                MainAudioManager.instance.SpawnAudio(rustleCue.GetSound(), Vector3.zero, .5f, 150, false, false);
                //Debug.Log("a");

                yield return new WaitForSeconds(3f);
            }
            else
            {
                q = currentQuests[i].success;
            }

            StartQuest(q);
            yield return new WaitForSeconds(.1f);
            if(q.conversations[0].autoContinue)
                ContinueQuest(q);
            currentQuests.Clear();
            currentChoices.Clear();
            DiceRollScreenManager.Finish(true, "Rolling...");
            DiceRollScreenManager.active = false;
        }

        public void StartQuest(Quest quest)
        {
            if (activeQuests.Any(activeQuest => activeQuest.quest == quest) || finishedQuests.Any(finishedQuest => finishedQuest == quest))
            {
                Debug.Log("Quest already finished");
                return;
            }

            if (quest.fight != null)
            {
                Instantiate(quest.fight);
                finishedQuests.Add(quest);
                return;
            }

            Debug.Log($"Started {quest.name} quest");
            activeQuests.Add(new ActiveQuest(quest, 0));
        }

        [SerializeField]
        private int _currentPos;
        private ActiveQuest _currentQuest;
        
        public void ContinueQuest(Quest quest)
        {
            ActiveQuest q = GetActiveQuest(quest);
            if (q == null)
            {
                Debug.LogError($"Quest {quest} is not active!!");
                return;
            }
            
            //check for needed items
            if (quest.conversations[q.pos].neededItems.Count > 0)
            {
                if (quest.conversations[q.pos].neededItems.Any(questNeededItem => questNeededItem.amount > InventoryManagerScript.instance.GetItemAmount(questNeededItem.item)))
                {
                    talkingToSelf = true;
                    TalkToSelf(new DialogueLine[] {new DialogueLine(player, "I don't have the items I need!")});
                    return;
                }
            }
            
            MainAudioManager.instance.SpawnAudio(rustleCue.GetSound(), Camera.main.transform.position, rustleCue.volume, 150, false, false);
            _currentQuest = q;
            IsTalking = true;
            _currentPos = 0;
            NextLine();
            
        }

        public void QuestPartFinished()
        {
            _currentQuest.pos++;
            _currentPos = 0;

            if (_currentQuest.quest.conversations.Count <= _currentQuest.pos)
            {
                if (_currentQuest.quest.choiceTexts.Length > 0)
                {
                    currentChoices = _currentQuest.quest.choiceTexts.ToList();
                    currentQuests = _currentQuest.quest.choiceQuests.ToList();
                    ShowOptions();
                    Debug.Log("hmmmmmmmmmmmmm");
                }else if (_currentQuest.quest.choiceQuests.Length > 0)
                {
                    currentQuests = new[] {_currentQuest.quest.choiceQuests[0]}.ToList();
                    Debug.Log("hmm");
                    StartCoroutine(Choice(0, _currentQuest.quest.choiceQuests[0].skill));
                }
                QuestSuccess(_currentQuest, this, EventArgs.Empty);
                _currentQuest = null;
                IsTalking = false;
                return;
            }

            //if (_currentPos == 0) return;
            if (_currentQuest.quest.conversations[_currentQuest.pos].showCG != null)
            {
                GameObject g = Instantiate(_currentQuest.quest.conversations[_currentQuest.pos].showCG, CGObject.transform);
                MainMenuManagerScript.cgUnlock[g.GetComponent<IndexerScript>().index] = true;
                SaveManager.SaveCGs(Application.persistentDataPath + "/cgdata.zues", MainMenuManagerScript.cgUnlock);
                CGManager.IsInCG = true;
            }

            if (_currentQuest.quest.conversations[_currentQuest.pos].autoContinue)
                NextLine();
            else
            {
                _currentQuest = null;
                IsTalking = false;
            }
        }

        public void NextLine()
        {
            MainAudioManager.instance.SpawnAudio(writeCue.GetSound(), Camera.main.transform.position, writeCue.volume, 150, false, false);
            if (_currentQuest == null)
            {
                Debug.LogError("NO QUEST!");
                return;
            }
            if (_currentQuest.quest.conversations[_currentQuest.pos].GetLines().Length <= _currentPos)
            {
                QuestPartFinished();
                return;
            }
            
            DialogueLine line = _currentQuest.quest.conversations[_currentQuest.pos].GetLineByIndex(_currentPos);

            if (line.triggerEvent != "")
            {
                Debug.Log(line.triggerEvent);
                DialogueEvent(line.triggerEvent, this, EventArgs.Empty);
            }

            dialogueText.text = line.dialogue;
            dialogueName.text = line.speaker.GetName();
            dialogueName.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(line.speaker.GetName() != "");
            dialogueCharacterImage.sprite = line.speaker.GetSpriteOutline();
            _currentPos++;
        }

        private Quest GetQuestOnThrow(ChoiceQuest quest)
        {
            float skill = quest.skill switch
            {
                Skills.Strength => InventoryManagerScript.instance.skills[0],
                Skills.Dexterity => InventoryManagerScript.instance.skills[1],
                Skills.Constitution => InventoryManagerScript.instance.skills[2],
                Skills.Intelligence => InventoryManagerScript.instance.skills[3],
                Skills.Wisdom => InventoryManagerScript.instance.skills[4],
                Skills.Charisma => InventoryManagerScript.instance.skills[5],
                Skills.none => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };

            return Random.Range(0, 21)> quest.minThrow ? quest.success : quest.fail;// + Mathf.FloorToInt((skill-10)/2) 
        }

        public EventHandler questSuccessEventHandler;
        public EventHandler questFailEventHandler;
        public EventHandler questDialogueEventHandler;

        public Quest lastQuestSuccess;
        public void QuestSuccess(ActiveQuest quest, object o, EventArgs e)
        {
            lastQuestSuccess = quest.quest;
            activeQuests.Remove(quest);
            finishedQuests.Add(quest.quest);
            questSuccessEventHandler?.Invoke(o, e);
            MainAudioManager.instance.SpawnAudio(rustleCue.GetSound(), Camera.main.transform.position, rustleCue.volume, 150, false, false);
        }

        public Quest lastQuestFail;
        public void QuestFail(ActiveQuest quest, object o, EventArgs e)
        {
            lastQuestFail = quest.quest;
            activeQuests.Remove(quest);
            finishedQuests.Add(quest.quest);
            questFailEventHandler?.Invoke(o, e);           
            MainAudioManager.instance.SpawnAudio(rustleCue.GetSound(), Camera.main.transform.position, rustleCue.volume, 150, false, false);
        }

        public string lastQuestEventName;
        public void DialogueEvent(string eventName, object o, EventArgs e)
        {
            lastQuestEventName = eventName;
            questDialogueEventHandler?.Invoke(o, e);
        }

        private List<DialogueLine> selfTalk;
        private Vector2 forceDir;
        
        public void TalkToSelf(DialogueLine[] text, Vector2 forceDirection = new())
        {
            MainAudioManager.instance.SpawnAudio(rustleCue.GetSound(), Camera.main.transform.position, rustleCue.volume, 150, false, false);
            selfTalk = text.ToList();
            IsTalking = true;
            talkingToSelf = true;
            forceDir = forceDirection;
            NextSelfTalkLine();
        }

        public Vector2 NextSelfTalkLine()
        {
            MainAudioManager.instance.SpawnAudio(writeCue.GetSound(), Camera.main.transform.position, writeCue.volume, 150, false, false);
            if (selfTalk.Count > 0)
            {
                dialogueCharacterImage.sprite = selfTalk[0].speaker.GetSpriteOutline();
                dialogueName.text = selfTalk[0].speaker.GetName();
                dialogueText.text = selfTalk[0].dialogue;
                selfTalk.RemoveAt(0);
                return new Vector2();
            }

            talkingToSelf = false;
            IsTalking = false;
            //Debug.Log(forceDir);
            return forceDir;
        }

        public ActiveQuest GetActiveQuest(Quest quest)
        {
            return activeQuests.FirstOrDefault(activeQuest => activeQuest.quest == quest);
        }

        public bool CheckFinishedQuests(IEnumerable<Quest> quests)
        {
            return quests.All(quest => finishedQuests.Contains(quest));
        }
    }
}