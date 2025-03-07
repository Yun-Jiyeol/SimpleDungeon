using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
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
            case ItemType.Potion:
            case ItemType.Resource:
                CharacterManager.Instance.Player.stat.AddItem(data);
                Destroy(gameObject);
                break;
            case ItemType.Weapon:
                CharacterManager.Instance.Player.interaction.GetWeapon(data.GO);
                break;
            case ItemType.Holdable:
                ItemUI.SetActive(false);
                CharacterManager.Instance.Player.controller.GetHoldableItemPosition(HoldPosition.transform.position);
                break;
            case ItemType.CarryAble:
                ItemUI.SetActive(false);
                CharacterManager.Instance.Player.interaction.CarryObject(this.gameObject);
                break;
        }
    }

    public void ControlUI(bool isOn)
    {
        if (ItemUI == null) return;
        ItemUI.SetActive(isOn);
    }
}
