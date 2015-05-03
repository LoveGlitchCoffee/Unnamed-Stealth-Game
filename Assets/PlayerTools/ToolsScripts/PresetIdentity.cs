using UnityEngine;
using System.Collections;

public class PresetIdentity : MonoBehaviour
{

    public int indexInDb;
    private Identifer _identifer;
    private GameObject _inventory;
	
    /*
     * Used for items starting off outside inventory
     * Presets their identity to item with index in database
     */
	void Start ()
	{
        _inventory  = GameObject.FindGameObjectWithTag("Inventory");
	    Tool newTool = _inventory.GetComponent<InventoryLogic>().ReturnToolDb().ToolDatabase[indexInDb];

	    _identifer = GetComponent<Identifer>();       

        _identifer.SetIdentity(newTool);
	}
	
	
}
