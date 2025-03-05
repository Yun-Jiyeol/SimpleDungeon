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
    public int SpeedPotion = 0;
    public bool isuseSpeedPotion = false;

    private UIStateController stateController;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
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
            case "SpeedPotion":
                SpeedPotion++;
                break;
        }
        stateController.UpdateItem();
    }

    public void OnUseHpPotion()
    {
        if(HpPotion <= 0)
        {
            return;
        }
        TakeSomethingToHp(gameManager.HealPotion.HeadAmount);
        HpPotion--;
        stateController.UpdateItem();
    }

    public void OnUseJumpPotion()
    {
        if (JumpPotion <= 0)
        {
            return;
        }
        CharacterManager.Instance.Player.controller.AddJump = gameManager.JumpPotion.AdditionalJump;
        JumpPotion--;
        stateController.UpdateItem();
    }
    public void OnUseSpeedPotion()
    {
        if (SpeedPotion <= 0)
        {
            return;
        }
        if(isuseSpeedPotion) return;

        isuseSpeedPotion = true;

        SpeedPotion--;
        stateController.UpdateItem();
        StartCoroutine(ChangeSpeed());
    }

    IEnumerator ChangeSpeed()
    {
        float maintain = gameManager.SpeedPotion.SpeedMaintain;
        float lefttime = maintain;
        CharacterManager.Instance.Player.controller.moveSpeed += gameManager.SpeedPotion.AdditionalSpeed;

        while (true)
        {
            lefttime -= Time.deltaTime;
            UIManager.Instance.StateController.SpeedMaintain.fillAmount = lefttime / maintain;
            yield return null;

            if(lefttime <= 0)
            {
                break;
            }
        }

        UIManager.Instance.StateController.SpeedMaintain.fillAmount = 1;
        CharacterManager.Instance.Player.controller.moveSpeed -= gameManager.SpeedPotion.AdditionalSpeed;
        isuseSpeedPotion = false;
    }
}
