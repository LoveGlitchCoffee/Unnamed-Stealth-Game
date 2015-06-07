using UnityEngine;
using System.Collections;

public class MenuSceneHandler : MonoBehaviour
{

    private Fading _fader;

    void Awake()
    {
        _fader = GetComponent<Fading>();
    }

    /*
     * Specific function for menu
     * Fades into game
     * Cannot use that of regular scene handler because conflicts in terms of facilities used, e.g. inventory doesn't exist here
     */
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
