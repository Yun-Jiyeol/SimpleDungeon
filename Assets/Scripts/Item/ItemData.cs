using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ItemType
{
    Heal,
    Addjump
}

public class ItemData : MonoBehaviour
{
    [Header("Info")]
    public string ItemName;
    public string ItemDescription;
    public ItemType type;

    [Header("Heal")]
    public float HeadAmount;

    [Header("AddJump")]
    public int AdditionalJump;

    public GameObject ItemUI;

    private void Start()
    {
        if(ItemUI != null)
        {
            ItemUI.SetActive(false);
        }
    }
    public string GetInteractPrompt()
    {
        string str = $"{ItemName}\n{ItemDescription}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.stat.AddItem(this);
        Destroy(gameObject);
    }

    public void ControlUI(bool isOn)
    {
        ItemUI.SetActive(isOn);
    }
}
