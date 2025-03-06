using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.MenuController = this;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnClickOffButton()
    {
        CharacterManager.Instance.Player.controller.OnOffMenu();
    }
}
