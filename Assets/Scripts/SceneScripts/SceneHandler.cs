using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour
{

    private Fading _sceneFader;    
    private GameObject _restart;
    private DescriptionWriter _writer;    
    public List<Object> RetainedObjects;
    private GameObject _player;
    private GameObject _cameraLead;
    private InventoryLogic _inventory;
    private Tool[] _savedTools;

    void Awake()
    {
        _sceneFader = GetComponent<Fading>();    
        _restart = transform.GetChild(0).gameObject;
        _writer = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponentInParent<DescriptionWriter>();
        _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryLogic>();
         _player = (GameObject)RetainedObjects[RetainedObjects.Count - 1];        //always place player last
         _cameraLead = (GameObject)RetainedObjects[RetainedObjects.Count - 2]; // always place lead next to last    
        
    }

    /*
     * Resets scene on button click
     * For objects that are not destroyed on scene load (mainly player),
     * Move them back to original position, set original layers and other setup like animation, also return inventory to previous state
     * Destroys retained objects if first level, as it already loads
     */
    public void ResetScene()
    {        
        int currentLevel = Application.loadedLevel;
        Application.LoadLevel(currentLevel);
        _restart.SetActive(false);
        StartCoroutine(_writer.WriteNarration(""));
        

        if (currentLevel == 1)
        {            
            for (int i = 0; i < RetainedObjects.Count; i++)
            {
                 Destroy(RetainedObjects[i]);
            }    
        }

        if (_player.GetComponent<PlayerNPCRelation>().dead)
        {
            _player.GetComponent<PlayerNPCRelation>().dead = false;
            _player.transform.position = new Vector3(0, 0);
            ReloadInventory();
            _player.GetComponent<Animator>().SetBool("dead", false);
            _player.GetComponent<Movement>().enabled = true;
            _player.GetComponent<CircleCollider2D>().enabled = true;
            _player.layer = 11;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
            
        

        _sceneFader.FadeIn();
    }

    /*
     * Resets inventory to state that was saved
     */
    private void ReloadInventory()
    {
        for (int i = 0; i < 4; i++)
        {
            _inventory.RemoveItem(i);
        }

        for (int i = 0; i < _savedTools.Length; i++)
        {            
            if (_savedTools[i].Name != null)
            {                
                //Debug.Log("adding " + _savedTools[i].Name);
                int savedItemId = _savedTools[i].ItemId;
                _inventory.AddItem(savedItemId);
            }
        }
    }

    /*
     * Create lose effect and activates canvas group
     */
    public IEnumerator Restart()
    {                
        yield return StartCoroutine(_writer.WriteNarration("No wonder you were caught in the first place"));
        yield return new WaitForSeconds(1.5f);
        _sceneFader.FadeOut();
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        _restart.SetActive(true);
    }


    /*
     * Retains object that are not destroyed when scene load
     * Actual loading occurs in Fading script, else would not work as this is an event function
     */
    public void ToNextLevel()
    {
        for (int i = 0; i < RetainedObjects.Count; i++)
        {
            DontDestroyOnLoad(RetainedObjects[i]);
        }
        
        int currentLevel = Application.loadedLevel;        
        _sceneFader.FadeOut();

        StartCoroutine(_writer.WriteNarration(""));

        StartCoroutine(_sceneFader.FadeToNextLevel(3f, currentLevel, _player, _cameraLead));        
    }

    /*
     * Saves state of inventory into an array
     */
    public void SaveInventory()
    {
        _savedTools = new Tool[_inventory.PlayerTools.Length];

        for (int i = 0; i < _inventory.PlayerTools.Length; i++)
        {
            _savedTools[i] = _inventory.PlayerTools[i];
        }
    }
}
