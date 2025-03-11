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
        Hp += amount; //ü���� ��ȭ
        if (Hp <= 0) //�������� �޾� ü���� �ٴ��̸� ���
        {
            Destroy(gameObject);
        }
        else if (Hp > MaxHp) //ȸ���� �޾� ü���� �ִ�ü���� �ѱ� �� ���̱�
        {
            Hp = MaxHp;
        }
    }
}
