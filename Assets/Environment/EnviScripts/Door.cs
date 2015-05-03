using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractive
{
    private int keyIndex = 0;
    
    /*
     * Doors purpose is to load next scene, only if player's inventory has a key
     */
    public void PerformPurpose(InventoryLogic inventory)
    {        
        Tool key = inventory.ReturnToolDb().ToolDatabase[keyIndex];

        if (inventory.PlayerTools.Contains(key))
        {            
            StartCoroutine(FadeToNextLevel());
        }                   
    }

    /*
     * Currently doesn't fade
     */
    IEnumerator FadeToNextLevel()
    {
        float fadeTime = 1f;
        float currentTime = 0f;
        
        while (currentTime < fadeTime)
        {
                    
            currentTime += 0.1f;
            yield return null;
        }

        Application.LoadLevel(1);
    }

}
