using UnityEngine;
using System.Collections;

public class Identifer : MonoBehaviour
{

    private Tool _identity;
    private SpriteRenderer spriteRender;

    /*
     * Embodies a player tool (lootable) in a game object
     */
    void Awake()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
    }

    /*
     * Sets the identiy and image of the game object to that of the passed tool
     */
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
