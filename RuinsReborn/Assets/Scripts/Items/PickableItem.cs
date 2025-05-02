using UnityEngine;

public class PickableItem : Interactable
{
    public ItemData _data;
    Rigidbody rb;

    float rbCooldown = 5f;
    float currentCooldown = 0f;
    public override void Init(ItemData newData)
    {
        base.Init(newData);
        _data = newData;
        rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        gameObject.layer = 7;
    }
    public override void OnItemInteracted()
    {
        // Disable gameobject
        Debug.Log($"{gameObject.name} || Picked Up");
        Inventory.Instance.OnItemPickUp(this);
        gameObject.SetActive(false);

    }

    public void OnItemDropped(Vector3 position, Vector3 dropRotation, ItemData newData)
    {
        _data = newData;
        gameObject.SetActive(true);
        transform.position = position;
        rb.isKinematic = false;
        transform.eulerAngles = dropRotation;
        rb.velocity = Vector3.forward * 2f;
    }

    private void FixedUpdate()
    {
        DoRigidBodyCooldown();
    }

    void DoRigidBodyCooldown()
    {
        if (rb.isKinematic)
        {
            currentCooldown = 0f;
            return;
        }

        if (CheckVelocity())
        {
            if (currentCooldown < rbCooldown)
                currentCooldown += Time.deltaTime;
            else
                rb.isKinematic = true;
        }
        else
            currentCooldown = 0f;
    }

    bool CheckVelocity()
    {
        float x = rb.velocity.x;
        float y = rb.velocity.y;
        float z = rb.velocity.z;

        if (x < 0.1f && y < 0.1f && z < 0.1f)
            return true;
        else
            return false;
    }
}
