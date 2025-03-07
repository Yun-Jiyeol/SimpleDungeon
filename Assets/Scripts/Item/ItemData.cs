using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ItemType
{
    Potion,
    Weapon,
    Holdable,
    CarryAble,
    BreakAble,
    Resource
}

public enum ResourceType
{
    Wood,
    Stone
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string ItemName;
    public string ItemDescription;
    public ItemType type;

    [Header("Heal")]
    public float HeadAmount;

    [Header("AddJump")]
    public int AdditionalJump;

    [Header("Speed")]
    public int AdditionalSpeed;
    public float SpeedMaintain;

    [Header("Weapon")]
    public int Damage;
    public float Rate;
    public float Duration;
    public float UseStamina;
    public List<ResourceType> canbreak;
    public GameObject GO;

    [Header("BreakAble")]
    public ResourceType resourceType;
    public int Hp;
    public int fallHp;
    public GameObject fallResource;
}
