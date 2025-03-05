using UnityEngine;
using UnityEngine.UI;

public class UIStateController : MonoBehaviour
{
    public Image HPBar;
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
}
