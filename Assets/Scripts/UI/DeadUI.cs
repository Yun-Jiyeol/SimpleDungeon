using UnityEngine;

public class DeadUI : MonoBehaviour
{
    public GameObject UI;

    private void Start()
    {
        UI.SetActive(false);
    }

    public void Dead()
    {
        UI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        CharacterManager.Instance.Player.controller.canLook = false;
    }

    public void OnClickGameoverButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
