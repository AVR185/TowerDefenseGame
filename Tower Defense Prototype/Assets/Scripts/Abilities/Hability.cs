using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hability 
{
    public string abilityName;

    public float abilityArea = 3;

    public Color32 abilityColor;

    public Sprite icon;

    public float coolDown = 3;

    public int cost = 5;

    public enum AbilityType { HealTowers, FreezeEnemies, DamageEnemies};

    public AbilityType abilityType;

    public bool locked = false;

    public Canvas indicador;
}
