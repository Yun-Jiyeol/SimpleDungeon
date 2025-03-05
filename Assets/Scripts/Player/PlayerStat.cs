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
        HP += amount; //ü���� ��ȭ
        if(HP <= 0) //�������� �޾� ü���� �ٴ��̸� ���
        {
            HP = 0;
            isDead = true;
        }
        else if(HP > MaxHP) //ȸ���� �޾� ü���� �ִ�ü���� �ѱ� �� ���̱�
        {
            HP = MaxHP;
        }
        stateController.HpBarController();
    }
}
