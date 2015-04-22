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
