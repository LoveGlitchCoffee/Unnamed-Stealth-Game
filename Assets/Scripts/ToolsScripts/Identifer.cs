using UnityEngine;

public class Identifer : MonoBehaviour
{

    private Tool _identity;
    private SpriteRenderer _spriteRender;
    private AudioSource _audio;

    /*
     * Embodies a player tool (lootable) in a game object
     */
    void Awake()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _audio = GetComponent<AudioSource>();
    }

    /*
     * Sets the identiy and image of the game object to that of the passed tool
     */
    public void SetIdentity(Tool identityTool)
    {
        _identity = identityTool;
        SetImage();
        SetPickUpSound();
        //SetSortingLayer();
    }

    public void SetPickUpSound()
    {
        _audio.clip = _identity.PickUpSound;        
    }

    public void SetDropSound()
    {
        _audio.clip = _identity.DropSound;        
    }

    public AudioClip ReturnSound()
    {
        return _audio.clip;
    }

    public void PlaySound()
    {
        _audio.Play();
    }
    

    public string ReturnIdentity()
    {
        return _identity.Name;
    }

    private void SetSortingLayer()
    {
        _spriteRender.sortingLayerName = "Environment";
        _spriteRender.sortingOrder = 1;
    }

    private void SetImage()
    {
        _spriteRender.sprite = _identity.ItemImage;
    }

    public Tool GetIdentity()
    {
        return _identity;
    }
}
