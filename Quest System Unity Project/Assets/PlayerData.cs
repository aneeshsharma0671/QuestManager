using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public int PlayerHealth = 50;
    public int EnemiesKilled = 0;
    public int ResourcesCollected = 0;
    public delegate void mydelegate(int x);
    public mydelegate OnPlayerHealthChange;
    public mydelegate OnEnemiesKilled;
    public mydelegate OnResourcesCollected;
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
    public void TakeDamage(int damage)
    {
        PlayerHealth -= damage;
        if(OnPlayerHealthChange != null)
        OnPlayerHealthChange(PlayerHealth);
    }
    public void Recover(int heal)
    {
        PlayerHealth += heal;
        if(OnPlayerHealthChange != null)
        OnPlayerHealthChange(PlayerHealth);
    }
    public void KillEnemy()
    {
        EnemiesKilled += 1;
        if(OnEnemiesKilled != null)
        OnEnemiesKilled(EnemiesKilled);
    }
    public void CollectResource()
    {
        ResourcesCollected += 1;
        if(OnResourcesCollected != null)
        OnResourcesCollected(ResourcesCollected);
    }
}
