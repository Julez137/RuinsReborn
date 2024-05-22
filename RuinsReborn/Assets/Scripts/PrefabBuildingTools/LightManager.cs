using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public bool enableLights;
    Light[] lights;
    LightManager[] managerChildren;
    private void OnValidate()
    {
        managerChildren = GetComponentsInChildren<LightManager>();

        if (managerChildren != null)
        {
            foreach (var manager in managerChildren)
                manager.EnableLights(enableLights);
        }
        else
        {
            lights = GetComponentsInChildren<Light>();
            EnableLights(enableLights);
        }
    }

    public void EnableLights(bool isEnabled)
    {
        enableLights = isEnabled;
        if (lights == null) return;
        foreach (var light in lights)
                light.enabled = enableLights;
    }
}
