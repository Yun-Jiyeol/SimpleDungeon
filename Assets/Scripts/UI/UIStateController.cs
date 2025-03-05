using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStateController : MonoBehaviour
{
    public Image HPBar;
    public TextMeshProUGUI HealthPotion;
    public TextMeshProUGUI JumpPotion;
    public TextMeshProUGUI SpeedPotion;
    public Image SpeedMaintain;

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

    public void UpdateItem()
    {
        HealthPotion.text = Stat.HpPotion.ToString();
        JumpPotion.text = Stat.JumpPotion.ToString();
        SpeedPotion.text = Stat.SpeedPotion.ToString();
    }
}
