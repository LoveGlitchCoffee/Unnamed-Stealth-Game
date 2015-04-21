using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryLogic : MonoBehaviour {

    [HideInInspector] public List<Tool> PlayerTools = new List<Tool>();
    private ToolDB _toolDb;

	void Awake () {
	    _toolDb = new ToolDB();   
	}

    void Start()
    {
        AddItem(0);
        //AddItem(1);
    }
	
	// Update is called once per frame
	void Update () {
	
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
                break;
            }
        }
    }
}
