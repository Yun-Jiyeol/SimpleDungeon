using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : ElectricEnd
{
    [Header("Lantern")]
    public GameObject Light;

    protected override void Update()
    {
        base.Update();

        if (isOn)
        {
            Light.gameObject.SetActive(true);
        }
        else
        {
            Light.gameObject.SetActive(false);
        }
    }
}
