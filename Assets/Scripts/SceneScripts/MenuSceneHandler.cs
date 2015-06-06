using UnityEngine;
using System.Collections;

public class MenuSceneHandler : MonoBehaviour
{

    private Fading _fader;

    void Awake()
    {
        _fader = GetComponent<Fading>();
    }

    public void ToGame()
    {
        _fader.FadeOut();

        StartCoroutine(WaitToFade());
    }

    private IEnumerator WaitToFade()
    {
        const float waitTime = 3f;
        float currentTime = 0f;

        while (currentTime < waitTime)
        {
            currentTime += 0.5f;
            yield return null;
        }
        
        Application.LoadLevel(1);

        _fader.FadeIn();
    }

    
}
