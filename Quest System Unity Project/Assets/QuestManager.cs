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
                PlayerData.Instance.OnPlayerHealthChange += quest.checkQuest;
                quest.OnSuccess += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.checkQuest; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.checkQuest; RemoveQuest(quest.QuestID); };
                break;
            case ConditionType.EnemiesKilled:
                PlayerData.Instance.OnEnemiesKilled += quest.checkQuest;
                quest.OnSuccess += () => { PlayerData.Instance.OnEnemiesKilled -= quest.checkQuest; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.checkQuest; RemoveQuest(quest.QuestID); };
                break;
            case ConditionType.ResourcesCollected:
                PlayerData.Instance.OnResourcesCollected += quest.checkQuest;
                quest.OnSuccess += () => { PlayerData.Instance.OnResourcesCollected -= quest.checkQuest; RemoveQuest(quest.QuestID); };
                quest.OnFail += () => { PlayerData.Instance.OnPlayerHealthChange -= quest.checkQuest; RemoveQuest(quest.QuestID); };
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
            Debug.Log("Quest Not Found " + questID);
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

    public void checkQuest(int currentCount)
    {
        Debug.Log("Checking Quest " + QuestID);
        switch (SuccessCondition.conditionType)
        { 
            case ConditionType.HealthGreaterThan:
                if (currentCount > SuccessCondition.conditionValue)
                {
                    OnSuccess();
                }
                break;
            case ConditionType.HealthLessThan:
                if (currentCount < SuccessCondition.conditionValue)
                {
                    OnSuccess();
                }
                break;
            case ConditionType.EnemiesKilled:
                if (currentCount > SuccessCondition.conditionValue)
                {
                    OnSuccess();
                }
                break;
            case ConditionType.ResourcesCollected:
                if (currentCount > SuccessCondition.conditionValue)
                {
                    OnSuccess();
                }
                break;
            default:
                break;
        }
        switch (FailCondition.conditionType)
        {
            case ConditionType.HealthGreaterThan:
                if (currentCount > FailCondition.conditionValue)
                {
                    OnFail();
                }
                break;
            case ConditionType.HealthLessThan:
                if (currentCount < FailCondition.conditionValue)
                {
                    OnFail();
                }
                break;
            case ConditionType.EnemiesKilled:
                if (currentCount > FailCondition.conditionValue)
                {
                    OnFail();
                }
                break;
            case ConditionType.ResourcesCollected:
                if (currentCount > FailCondition.conditionValue)
                {
                    OnFail();
                }
                break;
            default:
                break;
        }
    }
}