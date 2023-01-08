using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Dictionary<int,Quest> Quests = new Dictionary<int, Quest>();
    public void AddQuest(Quest quest)
    {
        BindQuest(quest);
        Quests.Add(quest.QuestID,quest);
        Debug.Log("Quest Added " + quest.Description);
    }

    void BindQuest(Quest quest)
    {
        switch (quest.SuccessCondition.conditionType)
        {
            case ConditionType.HealthGreaterThan :
            case ConditionType.HealthLessThan:
                PlayerData.Instance.OnPlayerHealthChange += quest.CheckSuccess;
                quest.OnSuccess += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckSuccess; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckSuccess; RemoveQuest(quest.QuestID); };
                break;
            case ConditionType.EnemiesKilled:
                PlayerData.Instance.OnEnemiesKilled += quest.CheckSuccess;
                quest.OnSuccess += () => { PlayerData.Instance.OnEnemiesKilled -= quest.CheckSuccess; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckSuccess; RemoveQuest(quest.QuestID); };
                break;
            case ConditionType.ResourcesCollected:
                PlayerData.Instance.OnResourcesCollected += quest.CheckSuccess;
                quest.OnSuccess += () => { PlayerData.Instance.OnResourcesCollected -= quest.CheckSuccess; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckSuccess; RemoveQuest(quest.QuestID); };
                break;
        }
        switch (quest.FailCondition.conditionType)
        {
            case ConditionType.HealthGreaterThan:
            case ConditionType.HealthLessThan:
                PlayerData.Instance.OnPlayerHealthChange += quest.CheckFail;
                quest.OnSuccess += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckFail; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckFail; RemoveQuest(quest.QuestID); };
                break;
            case ConditionType.EnemiesKilled:
                PlayerData.Instance.OnEnemiesKilled += quest.CheckFail;
                quest.OnSuccess += () => { PlayerData.Instance.OnEnemiesKilled -= quest.CheckFail; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckFail; RemoveQuest(quest.QuestID); };
                break;
            case ConditionType.ResourcesCollected:
                PlayerData.Instance.OnResourcesCollected += quest.CheckFail;
                quest.OnSuccess += () => { PlayerData.Instance.OnResourcesCollected -= quest.CheckFail; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckFail; RemoveQuest(quest.QuestID); };
                break;
        }
    }
    public void RemoveQuest(int questID)
    {
        if (Quests.ContainsKey(questID))
        {
            Quests.Remove(questID);
            Debug.Log("Quest Removed " + questID);
        }
        else
        {
            //Debug.Log("Quest Not Found " + questID);
        }
    }
    public void ShowQuests()
    {
        Debug.Log("Active Quests:");
        foreach (KeyValuePair<int,Quest> quest in Quests)
        {
            Debug.Log(quest.Value.Description);
        }
    }
}
// public class HealthCondition : QuestCondtion
// {
//     public override void BindPassCondition(Quest quest)
//     {
//         PlayerData.Instance.OnPlayerHealthChange += quest.CheckSuccess;
//         quest.OnSuccess += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckSuccess; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//         quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckSuccess; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//     }

//     public override void BindFailCondition(Quest quest)
//     {
//         PlayerData.Instance.OnPlayerHealthChange += quest.CheckFail;
//         quest.OnSuccess += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckFail; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//         quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckFail; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//     }
// }

// public class EnemiesKilledCondition : QuestCondtion
// {
//     public override void BindPassCondition(Quest quest)
//     {
//         PlayerData.Instance.OnEnemiesKilled += quest.CheckSuccess;
//         quest.OnSuccess += () => { PlayerData.Instance.OnEnemiesKilled -= quest.CheckSuccess; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//         quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckSuccess; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//     }

//     public override void BindFailCondition(Quest quest)
//     {
//         PlayerData.Instance.OnEnemiesKilled += quest.CheckFail;
//         quest.OnSuccess += () => { PlayerData.Instance.OnEnemiesKilled -= quest.CheckFail; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//         quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckFail; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//     }
// }

// public class ResourcesCollectedCondition : QuestCondtion
// {
//     public override void BindPassCondition(Quest quest)
//     {
//         PlayerData.Instance.OnResourcesCollected += quest.CheckSuccess;
//         quest.OnSuccess += () => { PlayerData.Instance.OnResourcesCollected -= quest.CheckSuccess; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//         quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckSuccess; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//     }

//     public override void BindFailCondition(Quest quest)
//     {
//         PlayerData.Instance.OnResourcesCollected += quest.CheckFail;
//         quest.OnSuccess += () => { PlayerData.Instance.OnResourcesCollected -= quest.CheckFail; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//         quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.CheckFail; QuestManager.Instance.RemoveQuest(quest.QuestID); };
//     }
// }
public enum ConditionType
{
    HealthGreaterThan,
    HealthLessThan,
    EnemiesKilled,
    ResourcesCollected,
}

[System.Serializable]
public struct QuestCondtion
{
    public ConditionType conditionType;
    public int conditionValue;
}
public class Quest
{
    public int QuestID { get; set; }
    public string Description { get; set; }
    public QuestCondtion SuccessCondition { get; set; }
    public Action OnSuccess { get; set; }
    public QuestCondtion FailCondition { get; set; }
    public Action OnFail { get; set; }

    public void CheckSuccess(int currentValue)
    {
        if (checkQuest(SuccessCondition,currentValue))
        {
            OnSuccess();
        }
    }
    public void CheckFail(int currentValue)
    {
        if (checkQuest(FailCondition,currentValue))
        {
            OnFail();
        }
    }
    bool checkQuest(QuestCondtion condition,int currentValue)
    {
        switch (condition.conditionType)
        {
            case ConditionType.HealthGreaterThan:
                return currentValue > condition.conditionValue;
            case ConditionType.HealthLessThan:
                return currentValue < condition.conditionValue;
            case ConditionType.EnemiesKilled:
                return currentValue > condition.conditionValue;
            case ConditionType.ResourcesCollected:
                return currentValue > condition.conditionValue;
        }
        return false;
    }
}