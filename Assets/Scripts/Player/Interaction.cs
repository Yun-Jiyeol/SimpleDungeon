using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;
    public GameObject RayPosition;

    public GameObject curInteractGameObject;
    private ItemObject curInteractable;

    public Transform CarryPosition;
    private bool isCarry = false;
    private GameObject CO;

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = new Ray(RayPosition.transform.position , RayPosition.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<ItemObject>();
                    curInteractable.ControlUI(true);
                }
            }
            else
            {
                if(curInteractable != null)
                {
                    curInteractable.ControlUI(false);
                }
                curInteractGameObject = null;
                curInteractable = null;
            }
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if(context.phase == InputActionPhase.Started && isCarry)
        {
            CO.GetComponent<Collider>().enabled = true;
            CO.GetComponent<Rigidbody>().useGravity = true;
            CO.GetComponent<Rigidbody>().AddForce(RayPosition.transform.forward * 1000f);
            isCarry = false;
        }
        else if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
        }
    }

    public void CarryObject(GameObject go)
    {
        isCarry = true;
        CO = go;
        CO.GetComponent<Collider>().enabled = false;
        CO.GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(CarryObjectPosition());
    }

    IEnumerator CarryObjectPosition()
    {
        while (isCarry)
        {
            CO.transform.position = CarryPosition.position;
            CO.transform.eulerAngles = CarryPosition.eulerAngles;
            yield return null;
        }
    }
}
