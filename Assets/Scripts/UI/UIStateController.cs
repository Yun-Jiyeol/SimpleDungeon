using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStateController : MonoBehaviour
{
    public Image HPBar;
    public Image StaminaBar;
    public TextMeshProUGUI HealthPotion;
    public TextMeshProUGUI JumpPotion;
    public TextMeshProUGUI SpeedPotion;
    public Image SpeedMaintain;
    public Image WeaponMaintain;
    public GameObject CrossHair;

    private PlayerStat Stat;

    private void Awake()
    {
        UIManager.Instance.StateController = this;
    }

    private void Start()
    {
        Stat = CharacterManager.Instance.Player.stat;
    }

    public void HpBarController()
    {
        HPBar.fillAmount = Stat.HP / Stat.MaxHP;
    }

    public void StaminaBarController()
    {
        StaminaBar.fillAmount = Stat.Stamina / Stat.MaxStamina;
    }

    public void UpdateItem()
    {
        HealthPotion.text = Stat.HpPotion.ToString();
        JumpPotion.text = Stat.JumpPotion.ToString();
        SpeedPotion.text = Stat.SpeedPotion.ToString();
    }
}
