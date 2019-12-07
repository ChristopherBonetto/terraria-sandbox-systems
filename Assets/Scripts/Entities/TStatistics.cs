using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TStatistics
{
    [Header("Defense variables")]
    public int MaxHealth;
    public int Defense;

    [Header("Fight variables")]
    public int Damage;
    public float AttackSpeed;
    [Range(0, 100)] public float KbResist;

    [Header("Movement variables")]
    public float Speed;
    public float JumpForce;


    public static TStatistics operator + (TStatistics a, TStatistics b)
    {
        TStatistics result = new TStatistics();

        result.MaxHealth = Mathf.Max(0, a.MaxHealth + b.MaxHealth);
        result.Defense = Mathf.Max(0, a.Defense + b.Defense);
        result.Damage = Mathf.Max(0, a.Damage + b.Damage);
        result.AttackSpeed = Mathf.Max(0, a.AttackSpeed + b.AttackSpeed);
        result.KbResist = Mathf.Max(0, a.KbResist + b.KbResist);
        result.Speed = Mathf.Max(0, a.Speed + b.Speed);
        result.JumpForce = Mathf.Max(0, a.JumpForce + b.JumpForce);

        return result;
    }

    public static TStatistics operator - (TStatistics a, TStatistics b)
    {
        TStatistics result = new TStatistics();

        result.MaxHealth = Mathf.Max(0, a.MaxHealth - b.MaxHealth);
        result.Defense = Mathf.Max(0, a.Defense - b.Defense);
        result.Damage = Mathf.Max(0, a.Damage - b.Damage);
        result.AttackSpeed = Mathf.Max(0, a.AttackSpeed - b.AttackSpeed);
        result.KbResist = Mathf.Max(0, a.KbResist - b.KbResist);
        result.Speed = Mathf.Max(0, a.Speed - b.Speed);
        result.JumpForce = Mathf.Max(0, a.JumpForce - b.JumpForce);

        return result;
    }
}
