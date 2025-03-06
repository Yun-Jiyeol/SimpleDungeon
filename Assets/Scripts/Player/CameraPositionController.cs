using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPositionController : MonoBehaviour
{
    public List<Vector3> Positions;
    public List<Vector3> Rotations;

    private int num = 0;

    private void Start()
    {
        if (Positions.Count != Rotations.Count)
        {
            Debug.Log("카메라 위치 애러!");
        }
        ChangeCameraPosition();
    }

    public void OnChangeCameraPosition(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            num++;
            if (num == Rotations.Count)
            {
                num = 0;
            }
            ChangeCameraPosition();
        }
    }

    void ChangeCameraPosition()
    {
        transform.localPosition = Positions[num];
        transform.localEulerAngles = Rotations[num];
        switch (num)
        {
            case 0:
                UIManager.Instance.StateController.CrossHair.SetActive(true);
                break;
            case 1:
            case 2:
                UIManager.Instance.StateController.CrossHair.SetActive(false);
                break;
        }
    }
}
