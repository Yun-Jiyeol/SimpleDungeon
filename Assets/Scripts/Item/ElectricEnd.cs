using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnd : MonoBehaviour
{
    [Header("ElectricEnd")]
    public bool isOn = false;
    public List<GameObject> gets;

    protected virtual void Update()
    {
        if (gets == null) return;

        foreach (GameObject get in gets)
        {
            if(get.GetComponent<ElectricStart>().Long >= 1)
            {
                isOn = true;
            }
            else
            {
                isOn = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("ElectStart"))
        {
            foreach (GameObject obj in gets)
            {
                if (obj == other.gameObject)
                {
                    return;
                }
            }
            gets.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        gets.Remove(other.gameObject);
    }
}
