using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractive
{
    private int keyIndex = 0;
    

    public void PerformPurpose(InventoryLogic inventory)
    {        
        Tool key = inventory.ReturnToolDb().ToolDatabase[keyIndex];

        if (inventory.PlayerTools.Contains(key))
        {
            Debug.Log("you has the key");
            StartCoroutine(FadeToNextLevel());
        }            
        else
        {
                   
        }
    }

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
