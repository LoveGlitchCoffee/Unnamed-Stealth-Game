using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private GameObject slotOfItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        slotOfItem = gameObject.transform.parent.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
    
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    
    }
}
