using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Image _toolImage;

    [HideInInspector] public int ItemSlotNumber;
    private InventoryLogic _inventory;
    private InventoryRenderer _invenRender;
    private Tool _toolInSlot;

    void Awake()
    {
        _toolImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryLogic>();
        _invenRender = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryRenderer>();
    }

	void Start ()
	{
	    UpdateSlot();
	}

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_toolInSlot.Name != null)
        {
            _invenRender.ActivateToolTip(gameObject.GetComponent<RectTransform>().localPosition, _inventory.PlayerTools[ItemSlotNumber], _inventory.PlayerTools[ItemSlotNumber].ItemDescription);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_toolInSlot.Name != null)
        _invenRender.DisableToolTip();
    }

    public Tool ReturnToolInSlot()
    {
        return _toolInSlot;
    }
}
