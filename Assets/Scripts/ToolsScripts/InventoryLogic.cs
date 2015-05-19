using System.Collections.Generic;
using UnityEngine;

public class InventoryLogic : MonoBehaviour {

    [HideInInspector] public List<Tool> PlayerTools = new List<Tool>(); //consider make it tree for efficiency
    private ToolDB _toolDb;

	void Awake () {
	    _toolDb = new ToolDB();        
	}

    void Start()
    {                
        AddItem(0);
    }
    
    public ToolDB ReturnToolDb()
    {
        return _toolDb;
    }

    /**
     * Add a non-empty item which is in database
     */
    public void AddItem(int id)
    {
        for (int i = 0; i < _toolDb.ToolDatabase.Count; i++)
        {
            if (_toolDb.ToolDatabase[i].ItemId == id)
            {                
                Tool itemToAdd = _toolDb.ToolDatabase[i];                
                FindEmptySlotToPlace(itemToAdd);
                break;
            }
        }
    }

    private void FindEmptySlotToPlace(Tool item)
    {
        for (int i = 0; i < PlayerTools.Count; i++)
        {
            if (PlayerTools[i].Name == null)
            {                
                PlayerTools[i] = item;
                transform.GetChild(i).GetComponent<ItemUI>().UpdateSlot();
                break;
            }
        }
    }

    public void RemoveItem(int indexOfDestoryed)
    {
        PlayerTools.RemoveAt(indexOfDestoryed);

        for (int i = 0; i < PlayerTools.Count; i++)
        {
            if (PlayerTools[i].Name != null)
                transform.GetChild(i).GetComponent<ItemUI>().UpdateSlot();
        }
    }

}
