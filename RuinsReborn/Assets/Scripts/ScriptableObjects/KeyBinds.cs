using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "KeyBinds", menuName = "ScriptableObjects/KeyBinds")]
public class KeyBinds : ScriptableObject
{
    public KeyBind[] normalKeyBinds;
    public KeyBind[] uiKeyBinds;


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
            default :
                return false;
                break;
        }

        return (foundKey != KeyCode.None);
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

    
    
    [System.Serializable]
    public class KeyBind
    {
        public string actionName;
        public KeyCode key;
    }

    public enum KeyBindType
    {
        Normal,
        UI
    }
}
