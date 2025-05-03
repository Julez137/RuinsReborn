using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilderManager : MonoBehaviour
{
    public static WorldBuilderManager instance;
    [Tooltip("The terrain of the world")]
    [SerializeField] Terrain worldTerrain;
    [Tooltip("The data of the items in the world")]
    [SerializeField] GlobalItemsDataHolder globalItemsDataHolder;
    [Header("Runtime Info")]
    public List<PickableItemPool> pickableItemPool = new List<PickableItemPool>();

    [Tooltip("OPTOMISATION = The amount of loop iterations per frame will be excecuted. More iterations = less fps.")]
    [SerializeField] int iterationCount = 100;
    int currentIterations;
    AudioSource pickUpSoundSource;

    private void Awake()
    {
        if (instance == null) instance = this;
        pickUpSoundSource = GetComponentInChildren<AudioSource>();
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

                    newItem.Init(new ItemData(item));

                    AddToItemPool(newItem);
                }
                if (CheckIterations()) yield return new WaitForEndOfFrame();
            }
        }
    }

    public void DropItem(ItemData data, int dropAmount)
    {
        data.SetItemCount(dropAmount);

        Vector3 camPos = Controls.instance.mainCamera.transform.position;
        Vector3 dropPosition = new Vector3(camPos.x, camPos.y - 0.2f, camPos.z);
        Vector3 dropRotation = Controls.instance.mainCamera.transform.eulerAngles;

        PickableItem item = GetItemFromPool(data.refName);
        if (item == null)
        {
            Debug.LogError($"Item {data.refName} does not have an inactive item in the pool. Destroying item.\n" +
                $"[ADD FEATURE TO INSTANTIATE NEW ITEM TO THE POOL]");
        }
        item.OnItemDropped(dropPosition, dropRotation, new ItemData(data));

    }

    void AddToItemPool(PickableItem newItem)
    {
        // Add a new Item Pool if the item pool is empty
        if (pickableItemPool.Count <= 0)
        {
            pickableItemPool.Add(new PickableItemPool(newItem));
            return;
        }
        // Add the new item to the item pool if the name exists in the pool.
        bool doesItemExist = false;
        foreach (var itemPool in pickableItemPool)
        {
            if (newItem.refName == itemPool.refName)
            {
                doesItemExist = true;
                itemPool.AddItem(newItem);
                return;
            }
        }
        // Add a new item to the pool if the name does not exist in the pool
        if (!doesItemExist)
        {
            pickableItemPool.Add(new PickableItemPool(newItem));
            return;
        }
    }

    PickableItem GetItemFromPool(string refName)
    {
        foreach (var item in pickableItemPool)
        {
            if (item.refName == refName)
                return item.GetInActiveItem();
        }

        return null;
    }

    public void PlayPickupSound(AudioClip clip, Vector3 worldPosition)
    {
        float randomPitch = Random.Range(0.8f, 1.5f);
        pickUpSoundSource.pitch = randomPitch;

        pickUpSoundSource.transform.position = worldPosition;
        pickUpSoundSource.PlayOneShot(clip);
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

[System.Serializable]
public class PickableItemPool
{
    public string refName;
    public List<PickableItem> items = new List<PickableItem>();

    public PickableItemPool(PickableItem newItem)
    {
        refName = newItem.refName;
        items.Add(newItem);
    }

    public void AddItem(PickableItem newItem)
    {
        items.Add(newItem);
    }

    /// <summary>
    /// Returns an item that is disabled in the scene from this pool
    /// </summary>
    public PickableItem GetInActiveItem()
    {
        foreach (var item in items)
            if (!item.gameObject.activeInHierarchy)
                return item;
        
        return null;
    }
}
