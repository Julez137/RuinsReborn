using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilderManager : MonoBehaviour
{
    [Tooltip("The terrain of the world")]
    [SerializeField] Terrain worldTerrain;
    [Tooltip("The data of the items in the world")]
    [SerializeField] GlobalItemsDataHolder globalItemsDataHolder;

    [Tooltip("OPTOMISATION = The amount of loop iterations per frame will be excecuted. More iterations = less fps.")]
    [SerializeField] int iterationCount = 100;
    int currentIterations;

    private void Awake()
    {
        if (globalItemsDataHolder == null) globalItemsDataHolder = Resources.Load<GlobalItemsDataHolder>("ScriptableObjects/GlobalItemsDataHolder");
    }

    private void Start()
    {
        Debug.Log($"{gameObject.name} || Assign world objects data");

        StartCoroutine(InitWorldObjectData());
    }

    IEnumerator InitWorldObjectData()
    {
        Collider[] allObjects = worldTerrain.GetComponentsInChildren<Collider>();
        yield return new WaitForEndOfFrame();
        foreach (Collider thisObject in allObjects)
        {
            Debug.Log($"{gameObject.name} || Check Object {thisObject.name}");
            foreach (var item in globalItemsDataHolder.items)
            {
                if (thisObject.name.Contains(item.refName))
                {
                    PickableItem newItem = thisObject.gameObject.AddComponent<PickableItem>();
                    newItem.Init(item);
                }
                if (CheckIterations()) yield return new WaitForEndOfFrame();
            }
        }
    }

    bool CheckIterations()
    {
        if (currentIterations >= iterationCount)
        {
            currentIterations = 0;
            return true;
        }
        else
        {
            currentIterations++;
            return false;
        }
    }
}
