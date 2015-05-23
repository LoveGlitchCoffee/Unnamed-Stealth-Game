using System.Collections;
using UnityEngine;

public class Loot : MonoBehaviour
{

    private readonly string _interTag = "Lootable";
    private bool _canLoot;
    private int _lootableId;
    private GameObject _destroyable;
    private const KeyCode _loot = KeyCode.E;
    private GameObject _inventory;

    void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("Inventory");
    }

    /**
     * Player will loot item in E is pressed
     */
    void Update()
    {

        if (_canLoot && Input.GetKeyDown(_loot))
        {
            _canLoot = false;
            StartCoroutine(LootItem());
        }
    }

    /*
     * Adds the item into inventory and destroy it in the game world
     */
    private IEnumerator LootItem()
    {
        _destroyable.GetComponent<Identifer>().PlaySound();                
        yield return StartCoroutine(WaitForSoundPlay(_destroyable.GetComponent<AudioSource>()));

        TransferToInvetory();
    }

    private void TransferToInvetory()
    {
        if (_inventory.GetComponent<InventoryLogic>().AddItem(_lootableId))
        {
            Destroy(_destroyable);            
        }        
    }


    private IEnumerator WaitForSoundPlay(AudioSource _audioSource)
    {
        while (_audioSource.isPlaying)
        {
            yield return null;
        }
    }

    /*
     * If collides with an lootable item, allow looting
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == _interTag)
        {
            _canLoot = true;            
            _lootableId = col.gameObject.GetComponent<Identifer>().GetIdentity().ItemId;           
            _destroyable = col.gameObject;
        }
    }

    
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == _interTag)
        {
            _canLoot = false;
        }
    }
}
