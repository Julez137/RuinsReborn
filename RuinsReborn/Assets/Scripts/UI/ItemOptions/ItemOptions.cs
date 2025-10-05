using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemOptions : MonoBehaviour
{
    public static ItemOptions Instance;
    [Header("Item Options Window")]
    [SerializeField] private Button _btnDropMultiple;
    [SerializeField] private Button _btnDropAll;
    
    [Header("Drop Multiple Items Window")]
    [SerializeField] private LeanWindow _dropMultipleWindow;
    [SerializeField] private TMP_InputField _inputDropMultiple;
    [SerializeField] private Button _btnDropItems;
    
    private LeanWindow _window;
    private EncapsulatedItem _item;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        _window = GetComponent<LeanWindow>();

        _btnDropMultiple.onClick.AddListener(OpenDropMultipleWindow);
        _btnDropAll.onClick.AddListener(DropAll);
        _btnDropItems.onClick.AddListener(DropItems);
    }

    public void Open(EncapsulatedItem item, Vector2 position)
    {
        GetComponent<RectTransform>().position = position;
        _item = item;
        _window.TurnOn();
    }

    public void Close()
    {
        _item = null;
        _window.TurnOff();
        _dropMultipleWindow.TurnOff();
    }

    void OpenDropMultipleWindow()
    {
        _dropMultipleWindow.TurnOn();
    }

    void DropItems()
    {
        if (_item == null) return;
        
        int currentTextNumber = int.Parse(_inputDropMultiple.text);
        if (currentTextNumber <= 0) return;
        
        int itemCount = _item.Data().itemCount;
        if (currentTextNumber >= itemCount)
            Inventory.Instance.OnItemDropped(_item, itemCount);
        else
            Inventory.Instance.OnItemDropped(_item, currentTextNumber);
        
        Close();
    }

    void DropAll()
    {
        Inventory.Instance.OnItemDropped(_item, _item.Data().itemCount);
        Close();
    }
}
