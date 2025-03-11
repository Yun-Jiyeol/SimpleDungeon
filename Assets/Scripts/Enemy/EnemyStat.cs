using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    [Header("State")]
    public float Hp;
    private float MaxHp;
    public float WalkSpeed;
    public float RunSpeed;

    public void TakeSomethingToHp(float amount)
    {
        Hp += amount; //체력의 변화
        if (Hp <= 0) //데미지를 받아 체력이 바닥이면 사망
        {
            Destroy(gameObject);
        }
        else if (Hp > MaxHp) //회복을 받아 체력이 최대체력이 넘길 시 줄이기
        {
            Hp = MaxHp;
        }
    }
}
