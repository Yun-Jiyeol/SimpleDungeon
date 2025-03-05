using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Stat")]
    public float HP;
    public float MaxHP;
    public bool isDead = false;

    private UIStateController stateController;

    private void Start()
    {
        stateController = UIManager.Instance.StateController;
        stateController.HpBarController();
    }

    public void TakeSomethingToHp(float amount)
    {
        HP += amount; //체력의 변화
        if(HP <= 0) //데미지를 받아 체력이 바닥이면 사망
        {
            HP = 0;
            isDead = true;
        }
        else if(HP > MaxHP) //회복을 받아 체력이 최대체력이 넘길 시 줄이기
        {
            HP = MaxHP;
        }
        stateController.HpBarController();
    }
}
