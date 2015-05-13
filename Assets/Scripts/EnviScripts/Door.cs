using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractive
{
    private int keyIndex = 0;
    private Fading _fader;

    void Awake()
    {
        _fader = GameObject.FindGameObjectWithTag("SceneFade").GetComponent<Fading>();
    }

    /*
     * Doors purpose is to load next scene, only if player's inventory has a key
     */
    public void PerformPurpose(InventoryLogic inventory)
    {        
        Tool key = inventory.ReturnToolDb().ToolDatabase[keyIndex];

        if (inventory.PlayerTools.Contains(key))
        {            
            _fader.ToNextLevel();
        }                   
    }
        

}
