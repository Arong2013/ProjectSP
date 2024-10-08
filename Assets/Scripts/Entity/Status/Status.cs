using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class StatComponent
{
    public void RemoveModifiersFromSource(object source)
    {
        Type type = this.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(Stat))
            {
                Stat stat = field.GetValue(this) as Stat;
                if (stat != null)
                {
                    stat.RemoveAllModifiersFromSource(source);
                }
            }
        }
    }
}

public static class StatusUtils
{
    public static void CombineStatus(List<StatComponent> targetComponents, List<StatComponent> sourceComponents, bool isAdd)
    {
        sourceComponents.ForEach(sourceComponent =>
            targetComponents.ForEach(targetComponent =>
                CombineStatus(targetComponent, sourceComponent, isAdd)));
    }

    public static void CombineStatus<T>(T target, T source, bool isAdd) where T : StatComponent
    {
        if (target.GetType() != source.GetType())
        {
            Debug.LogWarning("Cannot combine different types of stat components.");
            return;
        }

        if (isAdd)
            CombineFields(target, source);
        else
            target.RemoveModifiersFromSource(source);
    }

    private static void CombineFields<T>(T target, T source)
    {
        Type type = typeof(T);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(Stat))
            {
                Stat targetStat = field.GetValue(target) as Stat;
                Stat sourceStat = field.GetValue(source) as Stat;
                if (targetStat != null && sourceStat != null)
                {
                    targetStat.AddModifier(new StatModifier(sourceStat.Value, StatModType.Flat, 1, source));
                }
            }
        }
    }
}

[System.Serializable]
public class CombatStats : StatComponent
{
    public Stat maxHP = new Stat(100);
    public Stat currentHP = new Stat(100);
    public Stat speed = new Stat(5);
    public Stat attack = new Stat(10);
    public Stat defense = new Stat(5);
    public Stat criticalChance = new Stat(5);
}

[System.Serializable]
public class SurvivalStats : StatComponent
{
    public Stat experience = new Stat(0);
    public Stat level = new Stat(1);
    public Stat stamina = new Stat(100);
    public Stat oxygen = new Stat(100);
    public Stat hunger = new Stat(100);
    public Stat thirst = new Stat(100);
    public Stat radiationLevel = new Stat(0);
    public Stat temperature = new Stat(37);
    public Stat inventoryCapacity = new Stat(20);
}

[System.Serializable]
public class Attributes : StatComponent
{
    public Stat strength = new Stat(10);
    public Stat intelligence = new Stat(10);
    public Stat stamina = new Stat(10);
}

[System.Serializable]
public class SkillStats : StatComponent
{
    public Stat smallGuns = new Stat(0);
    public Stat bigGuns = new Stat(0);
    public Stat energyWeapons = new Stat(0);
    public Stat explosives = new Stat(0);
    public Stat meleeWeapons = new Stat(1);
    public Stat unarmed = new Stat(0);
    public Stat throwing = new Stat(0);
}
