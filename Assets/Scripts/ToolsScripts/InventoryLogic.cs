using System.Collections.Generic;
using UnityEngine;

public class InventoryLogic : MonoBehaviour
{

    [HideInInspector] public Tool[] PlayerTools = new Tool[4];
    private ToolDB _toolDb;

	void Awake () {
	    _toolDb = new ToolDB();        
	}

    void Start()
    {                
        //AddItem(0);
    }
    
    public ToolDB ReturnToolDb()
    {
        return _toolDb;
    }

    /**
     * Add a non-empty item which is in database
     */
    public bool AddItem(int id)
    {
        bool canPlaced = true;
        
        for (int i = 0; i < _toolDb.ToolDatabase.Count; i++)
        {
            if (_toolDb.ToolDatabase[i].ItemId == id)
            {                
                Tool itemToAdd = _toolDb.ToolDatabase[i];                
                canPlaced = FindEmptySlotToPlace(itemToAdd);
                break;
            }
        }
        
        Debug.Log("can place " + canPlaced);
        return canPlaced;

    }

    private bool FindEmptySlotToPlace(Tool item)
    {        
        bool placed = false;
        int count = 0;
        

        while (!placed && count < 4)
        {            
            if (PlayerTools[count].Name == null)
            {
                Debug.Log("adding item");
                PlayerTools[count] = item;
                transform.GetChild(count).GetComponent<ItemUI>().UpdateSlot();
                placed = true;
            }
            else
            {
                count++;
            }
        }

        return placed;

        /*for (int i = 0; i < PlayerTools.Length; i++)
        {
            if (PlayerTools[i].Name == null)
            {                
                PlayerTools[i] = item;
                transform.GetChild(i).GetComponent<ItemUI>().UpdateSlot();
                break;
            }            
        }*/
    }

    public void RemoveItem(int indexOfDestoryed)
    {
        PlayerTools[indexOfDestoryed] = new Tool();        

        /*for (int i = 0; i < PlayerTools.Length; i++)
        {            
                transform.GetChild(i).GetComponent<ItemUI>().UpdateSlot();
        }*/

        Debug.Log("remove at "+transform.GetChild(indexOfDestoryed).GetComponent<ItemUI>());
        transform.GetChild(indexOfDestoryed).GetComponent<ItemUI>().UpdateSlot();
    }


    /*
     * Should only be used if item does exist in inventory
     */
    public int IndexOf(Tool tool)
    {
        int index = -1;

        for (int i = 0; i < PlayerTools.Length; i++)
        {
            if (PlayerTools[i] == tool)
                index = i;
        }

        return index;
    }
}
