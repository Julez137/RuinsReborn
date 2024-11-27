using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterWindow : MonoBehaviour
{
    private LeanWindow window;
    private bool isWindowOpen = false;
    public bool IsWindowOpen { get => isWindowOpen;}

    private void Awake()
    {
        window = GetComponent<LeanWindow>();
    }

    void Start()
    {
        window.TurnOff();
    }

    public void EnableWindow()
    {
        isWindowOpen = !isWindowOpen;
        if (isWindowOpen)
            window.TurnOn();
        else
            window.TurnOff();
    }
}
