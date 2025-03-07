using System.Collections;
using UnityEngine;

public class Lebber : MonoBehaviour
{
    public bool isOn = false;
    public GameObject stick;
    public GameObject loading;
    public float loadingTime;

    private void Start()
    {
        changeStickRotation();
    }

    public void loadingLebber()
    {
        CharacterManager.Instance.Player.controller.canLook = false;
        StartCoroutine(loadingLebbercoroutine());
    }

    IEnumerator loadingLebbercoroutine()
    {
        while (true)
        {
            loading.transform.localScale += new Vector3 (1f, 1f, 1f) * Time.deltaTime / loadingTime;
            if(loading.transform.localScale.x >= 1f)
            {
                break;
            }
            yield return null;
        }

        isOn = !isOn;
        changeStickRotation();
        loading.transform.localScale = Vector3.zero;
        CharacterManager.Instance.Player.controller.canLook = true;
    }

    void changeStickRotation()
    {
        if (isOn)
        {
            stick.transform.localEulerAngles = new Vector3(-30,0,0);
        }
        else
        {
            stick.transform.localEulerAngles = new Vector3(30, 0, 0);
        }
    }
}
