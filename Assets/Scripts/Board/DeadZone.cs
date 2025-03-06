using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            other.gameObject.transform.position = GameManager.Instance.savePosition;
            other.gameObject.transform.eulerAngles = GameManager.Instance.saveRotation;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
