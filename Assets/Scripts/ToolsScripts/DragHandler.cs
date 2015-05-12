using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Tool _itemBeingDragged;
    private Vector3 _startPosition;
    public GameObject InteractiveObj;    
    private InventoryLogic _inventory;
    private GameObject  _slotIn, _physicalSpawn;
    private Vector3 screenPoint, offset;
    private DragSoundHandler _soundHandler;

    void Start()
    {
        _slotIn = transform.parent.gameObject;
        _inventory = _slotIn.transform.parent.GetComponent<InventoryLogic>();
        _soundHandler = GetComponent<DragSoundHandler>();
    }

    /*
     * When start drag, Instantiate a (lootable) game object that embodies the dragged item
     * Calculates where the item should appear in the game world according to the screen point
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        _itemBeingDragged = gameObject.transform.parent.gameObject.GetComponent<ItemUI>().ReturnToolInSlot();

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        /*offset = (gameObject.transform.position -
                  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z))); */ //for 3d

        SetUpPhysicalSpawn();

        _soundHandler.PlayDragSound();
    }

    private void SetUpPhysicalSpawn()
    {
        _physicalSpawn = Instantiate(InteractiveObj);
        _physicalSpawn.GetComponent<Identifer>().SetIdentity(_itemBeingDragged);
        _physicalSpawn.layer = 14;
        _physicalSpawn.transform.position =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    /*
     * Whilst dragging, change the instatiated object's position in game world according to screen point on mouse
     */
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 itemPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)); 
        _physicalSpawn.transform.position = itemPosition;        
    }

    /*
     * When drag end, enable physics property of object so that it 'becomes part of game world'
     * Remove the item from inventory
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        SetPhysicalProperties();

        StartCoroutine(_soundHandler.SetSoundProperties(_physicalSpawn));

        RemoveFromInventory();
    }

    private void RemoveFromInventory()
    {
        int indexOfDestoryed = _slotIn.GetComponent<ItemUI>().ItemSlotNumber;
        _inventory.RemoveItem(indexOfDestoryed);
    }
    


    private void SetPhysicalProperties()
    {
        _physicalSpawn.GetComponent<BoxCollider2D>().enabled = true;
        _physicalSpawn.GetComponent<CircleCollider2D>().enabled = true;
        _physicalSpawn.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
