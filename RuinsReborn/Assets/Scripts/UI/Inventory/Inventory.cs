using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private LeanWindow window;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null) Instance = this;
        window = GetComponent<LeanWindow>();
    }
    
    public bool InventoryPressed()
    {
        if (window.On)
        {
            window.TurnOff();
            // Inventory should close after Inventory button is pressed - Update isMenuOpen to false
            return false;
        }
        else
        {
            window.TurnOn();
            // Inventory should open after Inventory button is pressed - Update isMenuOpen to true
            return true;
        }
    }
}
