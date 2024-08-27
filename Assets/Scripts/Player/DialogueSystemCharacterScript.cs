using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemCharacterScript : MonoBehaviour
{
    public Speaker speaker;
    public List<string> choices;

    public ActiveQuest currQuest;
    public int currConversation;
    public void CheckActivity()
    {
        choices.Clear();

        foreach (ActiveQuest quest in MainDialogueManager.Instance.activeQuests)
        {
            int index = 0;
            Debug.Log(quest.quest);
            foreach (Conversation questConversation in quest.quest.conversations)
            {
                if (questConversation.questPart == quest.pos)
                {
                    if (questConversation.GetLines().Length > 1)
                    {
                        if (speaker != null)
                        {
                            if (speaker == questConversation.GetLineByIndex(0).speaker ||
                                speaker == questConversation.GetLineByIndex(1).speaker)
                            {
                                choices.Add(questConversation.choiceText);
                                currQuest = quest;
                                currConversation = index;
                            }
                        }
                        else
                        {
                            choices.Add(questConversation.choiceText);
                            currQuest = quest;
                            currConversation = index;
                        }
                    }
                    else
                    {
                        if (speaker == questConversation.GetLineByIndex(0).speaker)
                        {
                            choices.Add(questConversation.choiceText);
                            currQuest = quest;
                            currConversation = index;
                        }
                    }
                }
                index++;
            }
        }

    }
}
