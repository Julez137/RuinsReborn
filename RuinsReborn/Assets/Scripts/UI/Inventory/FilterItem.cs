using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterItem : MonoBehaviour
{
    [SerializeField] Sprite image;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
