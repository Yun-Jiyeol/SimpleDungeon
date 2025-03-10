using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWay : ElectricStart
{
    [Header("ElectricWay")]
    public List<GameObject> gets;

    public Material OnMaterial;
    public Material OffMaterial;
    public GameObject Pivot;

    private void Update()
    {
        float MaxgetElectric = 0;

        foreach (GameObject get in gets)
        {
            if (get.GetComponent<ElectricStart>().isOn)
            {
                if (MaxgetElectric < get.GetComponent<ElectricStart>().Long)
                {
                    MaxgetElectric = get.GetComponent<ElectricStart>().Long;
                }
            }
        }

        if(MaxgetElectric > 1)
        {
            isOn = true;
            Pivot.GetComponent<Renderer>().material = OnMaterial;
        }
        else
        {
            isOn = false;
            Pivot.GetComponent<Renderer>().material = OffMaterial;
        }
        Long = MaxgetElectric - 1;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("ElectStart"))
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
