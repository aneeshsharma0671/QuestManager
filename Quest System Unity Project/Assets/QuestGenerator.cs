using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class QuestGenerator : MonoBehaviour
{
    int currentQuestID = 1;
    public void InitialiseQuest(string description ,QuestCondtion successcondition,Action OnSuccess,QuestCondtion failCondition,Action OnFail = null)
    {
        Quest quest = new Quest();
        quest.QuestID = currentQuestID;
        quest.Description = description;
        quest.SuccessCondition = successcondition;
        quest.OnSuccess = OnSuccess;
        quest.FailCondition = failCondition;
        quest.OnFail = OnFail;
        QuestManager.Instance.AddQuest(quest);
        currentQuestID += 1;
    }

    #region TestMethods
    public int questIDToDelete;
    public String QuestDescription;
    public QuestCondtion successCondition;
    public QuestCondtion failCondition;

    [ContextMenu("Generate Quest")]
    public void GenerateQuest()
    {
        InitialiseQuest(QuestDescription, successCondition, () => { Debug.Log("Quest Completed :" + QuestDescription); }, failCondition, () => { Debug.Log("Quest Failed:" + QuestDescription); });
    }
    // [ContextMenu("Remove Quest")]
    // public void RemoveQuest()
    // {
    //     QuestManager.Instance.RemoveQuest(questIDToDelete);
    // }
    #endregion
}
