using System;
using System.Collections;
using System.Collections.Generic;
using MainManagers;
using ScriptableObjects;
using UnityEngine;
using State = ScriptableObjects.State;

public class FightTrigger : MonoBehaviour
{
    public bool infiniteBounds;
    public bool oneUse;
    public bool canFlee = true;
    public int staminaDrain = 0;
    public int turnsToLoss = -1;
    public State[] startStates;
    public EnemyCharacter[] enemies;
    public TriggerScript winTrigger;
    public TriggerScript lossTrigger;
    public TriggerScript fleeTrigger;

    private void Start()
    {
        if(infiniteBounds)
            StartFight();
    }

    private bool wasUsed = false;
    private void StartFight()
    {
        if(wasUsed) return;
        TurnBasedAlternativeManager.turnBasedAlternativeInstance.StartCombat(enemies, winTrigger, lossTrigger, fleeTrigger, canFlee, staminaDrain, startStates, turnsToLoss);
        if (oneUse)
            wasUsed = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        StartFight();
    }
}
