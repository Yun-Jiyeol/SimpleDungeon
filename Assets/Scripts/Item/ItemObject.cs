using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData data;
    public GameObject ItemUI;

    public TextMeshPro Name;
    public TextMeshPro Description;

    public GameObject HoldPosition;

    private void Start()
    {
        if (ItemUI != null)
        {
            Name.text = $"{data.ItemName}";
            Description.text = $"{data.ItemDescription}";
            ItemUI.SetActive(false);
        }
    }

    public void OnInteract()
    {
        switch (data.type)
        {
            case ItemType.Weapon:
            case ItemType.Potion:
                CharacterManager.Instance.Player.stat.AddItem(data);
                Destroy(gameObject);
                break;
            case ItemType.Holdable:
                CharacterManager.Instance.Player.controller.GetHoldableItemPosition(HoldPosition.transform.position);
                break;
        }
    }

    public void ControlUI(bool isOn)
    {
        if (ItemUI == null) return;
        ItemUI.SetActive(isOn);
    }
}
