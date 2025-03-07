using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoard : MonoBehaviour
{
    public float jumpForce;
    public float jumpTime;

    private Animator animator;
    private List<GameObject> Go = new List<GameObject>();
    private Coroutine jump;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Go.Add(collision.gameObject);
        jump = StartCoroutine(jumpcollider());
    }

    private void OnCollisionExit(Collision collision)
    {
        Go.Remove(collision.gameObject);
        StopCoroutine(jump);
        animator.SetBool("IsSomethingOn", false);
    }

    IEnumerator jumpcollider()
    {
        yield return new WaitForSeconds(jumpTime);
        animator.SetBool("IsSomethingOn", true);
        foreach(GameObject go in Go)
        {
            go.GetComponent<Rigidbody>().velocity = Vector3.zero;
            go.GetComponent<Rigidbody>().AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }
}
