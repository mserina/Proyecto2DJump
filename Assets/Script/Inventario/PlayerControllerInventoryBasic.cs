using ObjectPicker;
using UnityEngine;

public class PlayerControllerInventoryBasic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect Pickup collision with the pickup objects to trigger the pickup action
        if (other.CompareTag("Pickup"))
        {
            // Trigger Debug Message
            Debug.Log("Picked up: " + other.gameObject.name);


            // Check if the other game object has a IPickable component and call the PickUp method if it does
            IPickable pickable = other.gameObject.GetComponent<IPickable>();
            if (pickable != null)
            {
                pickable.PickUp();
                GameManagerInventory.instance.GetInventoryPicked().AddItem(pickable);
            }
        }
    }

}
