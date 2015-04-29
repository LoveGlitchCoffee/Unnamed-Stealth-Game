using UnityEngine;
using System.Collections;

public class Identifer : MonoBehaviour
{

    private Tool _identity;
    private SpriteRenderer spriteRender;

    void Awake()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetIdentity(Tool identityTool)
    {
        _identity = identityTool;
        SetImage();
        SetSortingLayer();
    }

    public string ReturnIdentity()
    {
        return _identity.Name;
    }

    private void SetSortingLayer()
    {
        spriteRender.sortingLayerName = "Environment";
        spriteRender.sortingOrder = 1;
    }

    private void SetImage()
    {
        spriteRender.sprite = _identity.ItemImage;
    }

    public Tool GetIdentity()
    {
        return _identity;
    }
}
