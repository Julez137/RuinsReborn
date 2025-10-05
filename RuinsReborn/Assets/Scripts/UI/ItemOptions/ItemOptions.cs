using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;

public class ItemOptions : MonoBehaviour
{
    public static ItemOptions Instance;
    private LeanWindow _window;
    private EncapsulatedItem _item;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        _window = GetComponent<LeanWindow>();
    }

    public void Open(EncapsulatedItem item)
    {
        _item = item;
        _window.TurnOn();
    }

    public void Close()
    {
        _item = null;
        _window.TurnOff();
    }
}
