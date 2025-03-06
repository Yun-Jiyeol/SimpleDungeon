using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float RunSpeed;
    public float RunStamina;
    private bool isRun = false;
    private bool isMove = false;
    public int AddJump;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public Transform head;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    [Header("Hold")]
    public Vector3 HoldableItem;
    public float HoldStamina;
    public float HoldSpeed;
    public float HoldJump;
    public float HoldDistance;
    private bool isHold = false;

    private bool isMenuOn = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (!canLook) return;
        if (isHold)
        {
            Hold();
        }
        else
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if (!canLook) return;
        CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (isRun)
        {
            if(CharacterManager.Instance.Player.stat.Stamina <= 0)
            {
                CharacterManager.Instance.Player.stat.useStamina = 0;
                dir *= moveSpeed;
            }
            else
            {
                if (isMove)
                {
                    CharacterManager.Instance.Player.stat.useStamina = RunStamina;
                }
                else
                {
                    CharacterManager.Instance.Player.stat.useStamina = 0;
                }
                dir *= RunSpeed;
            }
        }
        else
        {
            CharacterManager.Instance.Player.stat.useStamina = 0;
            dir *= moveSpeed;
        }
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    public void GetHoldableItemPosition(Vector3 lastposition)
    {
        HoldableItem = lastposition;
        AddJump = 1;
        isHold = true;
    }

    void Hold()
    {
        if(CharacterManager.Instance.Player.stat.Stamina <= 0)
        {
            isHold = false;
        }
        else
        {
            CharacterManager.Instance.Player.stat.useStamina = HoldStamina;
            if (!lengthbetweenPositions(HoldableItem,HoldDistance))
            {
                Vector3 vec = new Vector3(HoldableItem.x - transform.position.x, HoldableItem.y - transform.position.y, HoldableItem.z - transform.position.z);
                vec.Normalize();
                vec *= HoldSpeed;
                rb.velocity = vec;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
    bool lengthbetweenPositions(Vector3 nextPosition, float distance)
    {
        float x = nextPosition.x - transform.position.x;
        float y = nextPosition.y - transform.position.y;
        float z = nextPosition.z - transform.position.z;

        return (Mathf.Sqrt(x * x + y * y + z * z) < distance);
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        head.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        //카메라의 y축만 돌리고
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
        //카메라의 x축은 몸을 돌린다
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isMove = true;
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isMove = false;
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && (IsGrounded() || AddJump != 0))
        {
            float useStamina = 10f;
            if (CharacterManager.Instance.Player.stat.Stamina < useStamina) return;

            CharacterManager.Instance.Player.stat.UseStaminOneTime(useStamina);
            UIManager.Instance.StateController.StaminaBarController();

            if (!IsGrounded()) 
            {
                AddJump--;
            }

            if (isHold)
            {
                isHold = false;
                rb.AddForce(Vector2.up * HoldJump, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            }
        }
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRun = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRun= false;
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnOffMenu();
        }
    }

    public void OnOffMenu()
    {
        isMenuOn = !isMenuOn;

        if (isMenuOn) //켜질 때
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            UIManager.Instance.MenuController.gameObject.SetActive(true);
            canLook = false;
        }
        else //꺼질 때
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            UIManager.Instance.MenuController.gameObject.SetActive(false);
            canLook = true;
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.02f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.02f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.02f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.02f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
}
