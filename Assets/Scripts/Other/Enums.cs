using System;
using UnityEngine;

public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedArrayAttribute(string[] names) { this.names = names; }
}

public enum ItemSlots
{
    head, torso, legs, mainHand, offHand, back, neck, ring
}

public enum DamageType
{
    buldgeoning, piercing, slashing, fire, cold, necrotic, holy, acid
}

public enum AIState
{
    idle, searching, combat
}

[Serializable]
public enum Skills
{
    Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma, none
}

public enum EnemyTypes
{
    melee, ranged, dummy
}

public enum CriticalFailType
{
    self, randomFriend
}

//------------------Damage States--------------------

public enum BrainStates
{
    normal, concussion, bleeding 
}
public enum EyesStates
{
    normal, blind, bleeding, pirate
}
public enum MouthStates
{
    normal, gagged, bleeding
}
public enum NoseStates
{
    normal, bleeding, broken
}

public enum BreastsStates
{
    normal
}
public enum HeartStates
{
    normal, slow, fast
}
public enum LungsStates
{
    normal, half, bleeding, asthma
}
public enum StomachStates
{
    normal, bleeding, pierced
}

public enum ThighsStates
{
    normal, thicc, bleeding, broken
}
public enum KneesStates
{
    normal, destroyed
}
public enum CalfStates
{
    normal, broken, bleeding
}
public enum FeetStates
{
    normal, bleeding
}

public enum ShoulderStates
{
    normal, dislocated
}
public enum ElbowStates
{
    normal
}
public enum HandStates
{
    normal, destroyed
}
public enum FingerStates
{
    five, four, three, two, one, destroyed
}

//---------------------------------------

public enum EnemyDamageStates
{
    normal, bleeding, destroyed
}