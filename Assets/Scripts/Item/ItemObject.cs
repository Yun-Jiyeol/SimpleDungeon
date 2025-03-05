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

    private void Start()
    {
        if (ItemUI != null)
        {
            Name.text = $"{data.ItemName}";
            Description.text = $"{data.ItemDescription}";
            ItemUI.SetActive(false);
        }
    }
    public string GetInteractPrompt()
    {
        string str = $"{data.ItemName}\n{data.ItemDescription}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.stat.AddItem(data);
        Destroy(gameObject);
    }

    public void ControlUI(bool isOn)
    {
        ItemUI.SetActive(isOn);
    }
}
