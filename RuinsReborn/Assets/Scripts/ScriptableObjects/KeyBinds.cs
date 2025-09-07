using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "KeyBinds", menuName = "ScriptableObjects/KeyBinds")]
public class KeyBinds : ScriptableObject
{
    public KeyBind[] normalKeyBinds;
    public KeyBind[] uiKeyBinds;
    public KeyBind[] hotbarBinds;


    public KeyCode GetKeyBind(KeyBindType keyBindType, string keyAction)
    {
        switch (keyBindType)
        {
            case KeyBindType.Normal :
                return SearchKeyCodeInList(normalKeyBinds, keyAction);
                break;
            case KeyBindType.UI :
                return SearchKeyCodeInList(uiKeyBinds, keyAction);
                break;
            case KeyBindType.Hotbar :
                return SearchKeyCodeInList(hotbarBinds, keyAction);
                break;
            default :
                return KeyCode.None;
                break;
        }

        return KeyCode.None;
    }
    
    public bool DoesKeyBindExist(KeyBindType keyBindType, KeyCode keyComparer)
    {
        KeyCode foundKey = KeyCode.None;
        switch (keyBindType)
        {
            case KeyBindType.Normal :
                foundKey =  SearchKeyCodeInList(normalKeyBinds, keyComparer);
                break;
            case KeyBindType.UI :
                foundKey =  SearchKeyCodeInList(uiKeyBinds, keyComparer);
                break;
            case KeyBindType.Hotbar :
                foundKey = SearchKeyCodeInList(hotbarBinds, keyComparer);
                break;
            default :
                return false;
                break;
        }

        return (foundKey != KeyCode.None);
    }
    
    public int GetKeycodeIndexInList(KeyBindType keyBindType, KeyCode keyComparer)
    {
        switch (keyBindType)
        {
            case KeyBindType.Normal :
                return GetKeycodeIndexInList(normalKeyBinds, keyComparer);
                break;
            case KeyBindType.UI :
                return GetKeycodeIndexInList(uiKeyBinds, keyComparer);
                break;
            case KeyBindType.Hotbar :
                return GetKeycodeIndexInList(hotbarBinds, keyComparer);
                break;
            default :
                return -1;
                break;
        }

        return -1;
    }

    KeyCode SearchKeyCodeInList(KeyBind[] keyBinds, string keyAction)
    {
        foreach (var bind in keyBinds)
        {
            if (bind.actionName == keyAction) 
                return bind.key;
        }

        return KeyCode.None;
    }
    
    KeyCode SearchKeyCodeInList(KeyBind[] keyBinds, KeyCode keyComparer)
    {
        foreach (var bind in keyBinds)
        {
            if (bind.key == keyComparer) 
                return bind.key;
        }

        return KeyCode.None;
    }

    int GetKeycodeIndexInList(KeyBind[] keyBinds, KeyCode keyComparer)
    {
        for (int i = 0; i < keyBinds.Length; i++)
        {
            if (keyBinds[i].key == keyComparer) 
                return i;
        }

        return -1;
    }

    
    
    [System.Serializable]
    public class KeyBind
    {
        public string actionName;
        public KeyCode key;
    }

    public enum KeyBindType
    {
        Normal,
        UI,
        Hotbar
    }
}
