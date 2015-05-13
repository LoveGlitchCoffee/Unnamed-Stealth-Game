using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{

    public int StartX;
    
    private int _maxLoad = 4;

    public GameObject InventorySpace;    
    private InventoryLogic _iLogic;
    
    private const int GapBetweenSlots = 40;

    void Awake()
    {
        _iLogic = GetComponent<InventoryLogic>();        
    }
	
	void Start () {
        RenderInventory();
	}

    /*
     * Render the inventory slots in correct positions
     */
    public void RenderInventory()
    {
        int itemSlotNumber = 0;

        for (int i = 0; i < _maxLoad; i++)
        {
            
            StartX += ((int)InventorySpace.GetComponent<RectTransform>().rect.width + GapBetweenSlots);                

            GameObject newInventorySpace = Instantiate(InventorySpace);            
            newInventorySpace.GetComponent<ItemUI>().ItemSlotNumber = itemSlotNumber;
            itemSlotNumber++;

            _iLogic.PlayerTools.Add(new Tool()); // add empty item
            newInventorySpace.transform.name = "slot " + i;
            newInventorySpace.transform.SetParent(this.transform);
            newInventorySpace.GetComponent<RectTransform>().localPosition = new Vector2(StartX, 0);
        }
    }


}
