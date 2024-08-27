using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Attack Action", menuName = "Fight/New Attack Action")]
    public class AttackAction : ScriptableObject
    {
        [Header("Info")]
        public string actionName;
        [TextArea] public string actionDescription;
        [Header("Stats")]
        public int damage;
        public DamageType damageType;
        public List<State> bonusStates;
        public float armor;
        [Header("Cost + other")]
        public int staminaCost;
        public AudioCue soundEffect;
        public CriticalFail criticalFail;
    }
}
