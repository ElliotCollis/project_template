﻿using System;
using System.Collections;

public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}

[Serializable]
public class StatModifier
{
    public float Value;
    public StatModType Type;
    public int Order;
    public object Source;

    public StatModifier() { }
    public StatModifier(float value, StatModType type, int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }
    public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }
    public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
}

/*
 // not using this, but its a good list of things I might change.
public enum VariableToModify
{
    MoveSpeed,
    SpeedMultiplyer,
    TurnSpeed,
    TurnMultiplyer,
    BoostMultiplyer,
    MaxStamina,
    RefuleRate,
    BurnRate,
    Damage,
    BulletSpeed,
    FireRate,
    Aim,
    ShootStaminaCost,// is this needed?
    MaxHealth,
    iFrames,
}
*/
