using UnityEngine;

public class GameManagerInventory : MonoBehaviour
{
    // Inventory
    [SerializeField] private InventoryCount _inventoryCount;
    [SerializeField] private InventoryPicker _inventoryPicker;

    public static GameManagerInventory instance;

    private void Awake()
    {
        instance =  this;
    }
    private void Start()
    {
        if(_inventoryCount == null)
        {
            _inventoryCount = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryCount>();    
        }
    }

    public InventoryCount GetInventory()
    {
        return _inventoryCount;
    }
    
    public InventoryPicker GetInventoryPicked()
    {
        return _inventoryPicker;
    }

    
    
    
    
}
