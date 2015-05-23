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

    void Awake()
    {
        _sceneFader = GetComponent<Fading>();    
        _restart = transform.GetChild(0).gameObject;
        _writer = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponentInParent<DescriptionWriter>();
        

        _player = (GameObject) RetainedObjects[RetainedObjects.Count - 1];        //always place player last
        _cameraLead =  (GameObject) RetainedObjects[RetainedObjects.Count - 2]; // always place lead next to last
    }

    /*
     * Resets scene on button click
     */
    public void ResetScene()
    {        
        int currentLevel = Application.loadedLevel;
        Application.LoadLevel(currentLevel);

        if (currentLevel == 1)
        {
            for (int i = 0; i < RetainedObjects.Count; i++)
            {
                 Destroy(RetainedObjects[i]);
            }    
        }
        

        _sceneFader.FadeIn();
    }


    public IEnumerator Restart()
    {                
        yield return StartCoroutine(_writer.WriteNarration("No wonder you were caught in the first place"));
        yield return new WaitForSeconds(1.5f);
        _sceneFader.FadeOut();
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        _restart.SetActive(true);
    }

    public void ToNextLevel()
    {
        for (int i = 0; i < RetainedObjects.Count; i++)
        {
            DontDestroyOnLoad(RetainedObjects[i]);
        }

        int currentLevel = Application.loadedLevel;
        
        _sceneFader.FadeOut();

        StartCoroutine(_sceneFader.FadeToNextLevel(3f, currentLevel, _player, _cameraLead));    
    

    }
   

}
