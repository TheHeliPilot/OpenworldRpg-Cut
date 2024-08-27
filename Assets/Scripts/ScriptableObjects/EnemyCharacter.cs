using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class State
    {
        //for a d6 its just 6
        public Sprite image;
        public int damageDie;
        public bool stun;
        public bool block;
        public int maxRounds;
    }

    public class InFightState
    {
        public State state;
        public int roundsLeft;

        public InFightState(State state)
        {
            this.state = state;
            roundsLeft = state.maxRounds;
        }
    }

    [Serializable]
    public class EnemyAttack
    {
        public AttackAction attack;
        public int hitChance;
    }

    [CreateAssetMenu(fileName = "New Enemy", menuName = "Fight/New Enemy")]
    public class EnemyCharacter : ScriptableObject
    {
        public string enemyName;
        public Sprite enemySprite;
        public Sprite enemySpriteSelected;
        //[NamedArrayAttribute(new string[] { "Head", "Torso", "Legs", "MainHand", "OffHand" })]
        public string[] equipment;
        public int[] health = {0,0,0,0,0};
        public int[] hitChancePercent = {0,0,0,0,0};
        public EnemyAttack mainHandAttack;
        public EnemyAttack offHandAttack;
        public DamagePair[] resistances;
        public float mainHandChance;

    }
}