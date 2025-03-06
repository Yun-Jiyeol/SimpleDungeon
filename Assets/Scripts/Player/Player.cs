using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerStat stat;
    public Interaction interaction;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        stat = GetComponent<PlayerStat>();
        interaction = GetComponent<Interaction>();
    }
}
