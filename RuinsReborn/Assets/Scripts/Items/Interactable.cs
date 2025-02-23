using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected Collider thisCollider;
    [Tooltip("The name of the object. This will be displayed in UI menus")]
    public string objectName;
    [Tooltip("The text being displayed when the object can be interactable. Template: Press <Key> to [insert text here].")]
    public string interactableText;
    [Tooltip("The width of the text displayed. This will control the background of the text to highlight it when displayed.")]
    public float textWidth;

    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
        if (thisCollider == null) Debug.LogError($"{gameObject.name} || No collider found on this object. Add a collider to interact with it.");
    }
    public abstract void OnItemInteracted();

    public void EnableCollider(bool isEnabled)
    {
        thisCollider.enabled = isEnabled;
    }

    public void OnItemLook(KeyCode interactableKeyInput)
    {
        if (ToolTip.instance == null)
        {
            Debug.LogError($"{gameObject.name} || Tooltip component does not exist in scene.");
            return;
        }
        ToolTip.instance.ShowText($"Press '{interactableKeyInput}' to {interactableText}.", textWidth);

        if (Input.GetKeyDown(interactableKeyInput)) OnItemInteracted();
    }
}
