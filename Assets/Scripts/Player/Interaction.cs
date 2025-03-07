using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    private bool isWeapon = false;
    private bool isCarry = false;
    private GameObject CO;
    private float lastTime;

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
        if (context.phase == InputActionPhase.Started && isWeapon) //무기를 들었을 때
        {
            if (curInteractable != null)
            {
                if(curInteractable.data.type == ItemType.Potion || curInteractable.data.type == ItemType.Resource)
                {
                    curInteractable.OnInteract();
                    curInteractGameObject = null;
                    curInteractable = null;
                }
            }
            if(Time.time - lastTime > CO.GetComponent<ItemObject>().data.Rate)
            {
                lastTime = Time.time;
                CO.GetComponent<Animator>().SetTrigger("Attack");
                CO.GetComponent<Animator>().SetFloat("Speed", 1 / CO.GetComponent<ItemObject>().data.Rate);
                CharacterManager.Instance.Player.stat.UseStaminOneTime(CO.GetComponent<ItemObject>().data.UseStamina);

                if(curInteractable == null) return;
                if(curInteractable.data.type != ItemType.BreakAble) return;

                bool canbreak = false;
                foreach (ResourceType type in CO.GetComponent<ItemObject>().data.canbreak)
                {
                    if(type == curInteractable.data.resourceType)
                    {
                        canbreak = true;
                    }
                }
                if(!canbreak) return;

                curInteractGameObject.GetComponent<ResourceGetHit>().OnHit(CO.GetComponent<ItemObject>().data.Damage);
            }
        }
        else if (context.phase == InputActionPhase.Started && isCarry)
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

    public void GetWeapon(GameObject go)
    {
        isWeapon = true;
        lastTime = Time.time;
        CO = Instantiate(go, CarryPosition);
        CO.transform.SetParent(CarryPosition);
        CO.GetComponent<Collider>().enabled = false;
        CO.GetComponent<Animator>().SetBool("IsHandle", true);
        StartCoroutine(WeaponMaintain());
    }

    public void CarryObject(GameObject go)
    {
        isCarry = true;
        CO = go;
        CO.GetComponent<Collider>().enabled = false;
        CO.GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(CarryObjectPosition());
    }

    IEnumerator WeaponMaintain()
    {
        float leftTime = CO.GetComponent<ItemObject>().data.Duration;

        while (true)
        {
            leftTime -= Time.deltaTime;
            UIManager.Instance.StateController.WeaponMaintain.fillAmount = leftTime / CO.GetComponent<ItemObject>().data.Duration;
            if(leftTime < 0) break;
            yield return null;
        }

        UIManager.Instance.StateController.WeaponMaintain.fillAmount = 1;
        Destroy(CO);
        isWeapon =false;
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
