using UnityEngine;
using UnityEngine.UI;

public class FirstLoot : MonoBehaviour
{

    private InventoryRenderer _invenRenderer;
    private Image _inventoryPanel;

    void Awake()
    {
        GameObject inventory = GameObject.FindGameObjectWithTag("Inventory");
        _invenRenderer = inventory.GetComponent<InventoryRenderer>();
        _inventoryPanel = inventory.GetComponent<Image>();        
    }

}
