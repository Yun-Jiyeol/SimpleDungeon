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
    public float useStamina;

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

    private void Update()
    {
        if(useStamina > 0)
        {
            if (BreakTime != null)
            {
                StopCoroutine(BreakTime);
                BreakTime = null;
            }
            if(Stamina <= 0)
            {
                Stamina = 0;
                return;
            }

            Stamina -= useStamina * Time.deltaTime;
            stateController.StaminaBarController();
        }
        else
        {
            if (BreakTime == null)
            {
                BreakTime = StartCoroutine(recoverStamina());
            }
        }
    }

    public void UseStaminOneTime(float amount)
    {
        if(BreakTime != null)
        {
            StopCoroutine(BreakTime);
            BreakTime = null;
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
            GameManager.Instance.Dead();
        }
        else if(HP > MaxHP) //회복을 받아 체력이 최대체력이 넘길 시 줄이기
        {
            HP = MaxHP;
        }
        stateController.HpBarController();
    }

    public void TakeSomethingToHp(float amount, Vector3 way)
    {
        HP += amount; //체력의 변화
        CharacterManager.Instance.Player.controller.canLook = false;
        Invoke(nameof (InvokeCanlook), 1);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().AddForce(way.normalized * amount * 300f);

        if (HP <= 0) //데미지를 받아 체력이 바닥이면 사망
        {
            HP = 0;
            GameManager.Instance.Dead();
        }
        stateController.HpBarController();
    }

    void InvokeCanlook()
    {
        CharacterManager.Instance.Player.controller.canLook = true;
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
            default:
                Debug.Log($"You Get {item.name}");
                break;
        }
        stateController.UpdateItem();
    }

    public void OnUseHpPotion()
    {
        if (Time.timeScale == 0) return;
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
        if (Time.timeScale == 0) return;
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
        if (Time.timeScale == 0) return;
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

    IEnumerator recoverStamina()
    {
        yield return new WaitForSeconds(1f);
        while(Stamina < MaxStamina)
        {
            Stamina += HealStamina*Time.deltaTime;
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
        CharacterManager.Instance.Player.controller.RunSpeed += gameManager.SpeedPotion.AdditionalSpeed;

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
        CharacterManager.Instance.Player.controller.RunSpeed -= gameManager.SpeedPotion.AdditionalSpeed;
        isuseSpeedPotion = false;
    }
}
