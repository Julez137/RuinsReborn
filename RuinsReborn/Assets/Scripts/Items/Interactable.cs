using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected Collider thisCollider;
    [Tooltip("The name used to reference the gameobject in the scene. This is only to assign the data to the matched object, not for UI.")]
    public string refName;
    [Tooltip("The name of the object. This will be displayed in UI menus")]
    public string objectName;
    [Tooltip("The text being displayed when the object can be interactable. Template: Press <Key> to [interactableText].")]
    public string interactableText;
    [Tooltip("The width of the text displayed. This will control the background of the text to highlight it when displayed.")]
    public float textWidth;

    private MeshRenderer[] _renderers;
    private Material _matOutline;

    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
        if (thisCollider == null) Debug.LogError($"{gameObject.name} || No collider found on this object. Add a collider to interact with it.");
        _matOutline = Resources.Load<Material>("Shaders/Outline");
        _renderers = GetComponentsInChildren<MeshRenderer>(true);
    }
    public abstract void OnItemInteracted();

    public virtual void Init(ItemData newData)
    {
        refName = newData.refName;
        objectName = newData.objectName;
        interactableText = newData.interactableText;
        textWidth = newData.textWidth;
    }
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
        EnableOutline(true);

        if (Input.GetKeyDown(interactableKeyInput))
        {
            OnItemInteracted();
        }
    }

    public void EnableOutline(bool isEnabled)
    {
        foreach (var renderer in _renderers)
        {
            Material[] currentMaterials = renderer.materials;
            List<Material> foundOutlines = new List<Material>();
            foreach (var material in currentMaterials)
                if (material.name.Contains(_matOutline.name)) foundOutlines.Add(material);
            
            
            if (foundOutlines.Count <= 0 && isEnabled)
            {
                List<Material> newMaterials = currentMaterials.ToList();
                newMaterials.Add(_matOutline);
                renderer.sharedMaterials = newMaterials.ToArray();
            }
            else if (foundOutlines.Count > 0 && !isEnabled)
            {
                renderer.sharedMaterials = RemoveOutlines(currentMaterials);
            }
        }
    }

    Material[] RemoveOutlines(Material[] currentMaterials)
    {
        // Create a new list to store materials without outlines
        List<Material> result = new List<Material>();

        foreach (var mat in currentMaterials)
        {
            if (!mat.name.Contains(_matOutline.name))
            {
                 result.Add(mat);
            }
        }

        Debug.Log($"Result material count : {result.Count}");
        return result.ToArray();
    }
}
