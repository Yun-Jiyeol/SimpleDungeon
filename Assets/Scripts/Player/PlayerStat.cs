using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Stat")]
    public float HP;
    public float MaxHP;
    public bool isDead = false;
    public float Stamina;
    public float MaxStamina;
    public float HealStamina;
    private float useStamina;

    [Header("ItemHave")]
    public int HpPotion = 0;
    public int JumpPotion = 0;
    public int SpeedPotion = 0;
    public bool isuseSpeedPotion = false;

    private Coroutine BreakTime;

    private UIStateController stateController;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        stateController = UIManager.Instance.StateController;
        Invoke(nameof(LateStart), 0.1f);
    }

    void LateStart()
    {
        stateController.HpBarController();
        stateController.StaminaBarController();
        stateController.UpdateItem();
    }

    public void UseStaminOneTime(float amount)
    {
        if(BreakTime != null)
        {
            StopCoroutine(BreakTime);
        }

        Stamina -= amount;
        stateController.StaminaBarController();

        BreakTime = StartCoroutine(recoverStamina());
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

    public IEnumerator recoverStamina()
    {
        yield return new WaitForSeconds(1f);
        while(Stamina < MaxStamina)
        {
            Stamina += HealStamina;
            stateController.StaminaBarController();
            yield return null;
        }
        if(Stamina >= MaxStamina)
        {
            Stamina = MaxStamina;
            stateController.StaminaBarController();
        }
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
