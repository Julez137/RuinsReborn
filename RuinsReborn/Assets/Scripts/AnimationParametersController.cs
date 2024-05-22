using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationParametersController : MonoBehaviour
{
    public string[] parameterNames;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #region BOOL
    public void SetBool(string parameterName, bool value)
    {
        if (DoesNameExist(parameterName))
            animator.SetBool(parameterName, value);
        else
            Debug.LogError($"{gameObject.name} || Animation bool parameter <{parameterName}> does not exist!");
    }

    public void SetBoolTrue(string parameterName)
    {
        if (DoesNameExist(parameterName))
            animator.SetBool(parameterName, true);
        else
            Debug.LogError($"{gameObject.name} || Animation bool parameter <{parameterName}> does not exist!");
    }

    public void SetBoolFalse(string parameterName)
    {
        if (DoesNameExist(parameterName))
            animator.SetBool(parameterName, false);
        else
            Debug.LogError($"{gameObject.name} || Animation bool parameter <{parameterName}> does not exist!");
    }
    #endregion

    bool DoesNameExist(string value)
    {
        bool doesNameExist = false;
        foreach (var parameter in parameterNames)
        {
            if (parameter == value)
            {
                doesNameExist = true;
                break;
            }
        }

        return doesNameExist;
    }
}
