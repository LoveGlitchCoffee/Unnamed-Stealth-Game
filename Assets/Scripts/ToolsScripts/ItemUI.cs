using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler//, IPointerExitHandler
{

    private Image _toolImage;

    [HideInInspector] public int ItemSlotNumber;
    private InventoryLogic _inventory;
    private InventoryRenderer _invenRender;
    private Tool _toolInSlot;
    private Text _descriptionBox;    

    void Awake()
    {
        _toolImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryLogic>();
        _invenRender = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryRenderer>();
        _descriptionBox = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponent<Text>();
    }

	void Start ()
	{
	    UpdateSlot();
	}

    /*
     * Updates the data in the slot if a change has been made in inventory logic
     */
    public void UpdateSlot()
    {
        _toolInSlot = _inventory.PlayerTools[ItemSlotNumber];
    }
	
	//put in update for now, will change later
	void Update () {
        
	    if (_toolInSlot.ItemImage != null)
	    {
	        _toolImage.enabled = true;
	        _toolImage.sprite = _inventory.PlayerTools[ItemSlotNumber].ItemImage;
	    }
	    else
	    {
	        _toolImage.enabled = false;
	    }
	}

    /*
     * Fill description box with tool's descripton
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_toolInSlot.Name != null)
        {            
            _descriptionBox.text = _toolInSlot.ItemDescription;
        }
    }
   

    public Tool ReturnToolInSlot()
    {
        return _toolInSlot;
    }
}
