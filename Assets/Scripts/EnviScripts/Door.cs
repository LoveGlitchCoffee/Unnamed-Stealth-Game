using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractive
{
    private int keyIndex = 0;
    private SceneHandler _sceneHandler;

    void Awake()
    {
        _sceneHandler = GameObject.FindGameObjectWithTag("SceneFade").GetComponent<SceneHandler>();
    }

    /*
     * Doors purpose is to load next scene, only if player's inventory has a key
     */
    public void PerformPurpose(InventoryLogic inventory)
    {        
        Tool key = inventory.ReturnToolDb().ToolDatabase[keyIndex];

        if (inventory.IndexOf(key) != -1)
        {            
            _sceneHandler.ToNextLevel();
            inventory.RemoveItem(inventory.IndexOf(key));
        }                   
    }
        

}
