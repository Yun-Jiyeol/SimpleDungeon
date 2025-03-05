using UnityEngine;

public class JumpBoard : MonoBehaviour
{
    public float jumpForce;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        animator.SetBool("IsSomethingOn",true);
        collision.rigidbody.velocity = Vector3.zero;
        collision.rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionExit(Collision collision)
    {
        animator.SetBool("IsSomethingOn", false);
    }
}
