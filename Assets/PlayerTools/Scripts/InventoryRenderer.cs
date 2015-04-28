using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{

    public int StartX;
    
    private int _maxLoad = 4;

    public GameObject InventorySpace;    
    private InventoryLogic _iLogic;

    public GameObject ToolTip;
    private const int GapBetweenSlots = 40;

    void Awake()
    {
        _iLogic = GetComponent<InventoryLogic>();
       // _toolTip = GameObject.FindGameObjectWithTag("ToolTip");
    }
	
	void Start () {
	    RenderInventory();
	}

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

    /**
     * have to do separate because can't set enabled of gameobject
     */
    public void ActivateToolTip(Vector2 position, Tool tool, string description)
    {
        ToolTip.SetActive(true);
        ToolTip.GetComponent<RectTransform>().localPosition = new Vector3(position.x, gameObject.GetComponent<RectTransform>().localPosition.y);
        ToolTip.transform.GetChild(0).GetComponent<Text>().text = description;
    }

    public void DisableToolTip()
    {
        ToolTip.SetActive(false);
    }

}
