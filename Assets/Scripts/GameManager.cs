using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    public ItemData HealPotion;
    public ItemData JumpPotion;
    public ItemData SpeedPotion;
    public Vector3 savePosition;
    public Vector3 saveRotation;
    public DeadUI DeadUI;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Dead()
    {
        Time.timeScale = 0f;
        DeadUI.Dead();
    }
}
