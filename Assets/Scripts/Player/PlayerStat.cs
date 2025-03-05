using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Stat")]
    public float HP;
    public float MaxHP;
    public bool isDead = false;

    [Header("ItemHave")]
    public int HpPotion = 0;
    public int JumpPotion = 0;

    private UIStateController stateController;

    private void Start()
    {
        stateController = UIManager.Instance.StateController;
        stateController.HpBarController(); 
        stateController.UpdateItem(); 
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
    
    public void AddItem(ItemData item)
    {
        switch (item.name)
        {
            case "HealPotion":
                HpPotion++;
                break;
            case "JumpPotion":
                JumpPotion++;
                break;
        }
        stateController.UpdateItem();
    }
}
