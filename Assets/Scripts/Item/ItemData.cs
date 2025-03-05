using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ItemType
{
    Potion,
    Weapon
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
}
