using UnityEngine;
using System.Collections;

public class CoinLover : MonoBehaviour
{
    private const string lootTag = "Lootable";
    private const string coinName = "coin";

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == lootTag && col.GetComponent<Identifer>().ReturnIdentity() == coinName)
        {
            Debug.Log("ohh a coin");
            StartCoroutine(AdmireCoin(col.gameObject));
        }
    }

    IEnumerator AdmireCoin(GameObject coin)
    {
        const float maxTime = 7f;
        float timer = 0f;

        //could shorten if put them with interface
        GuardAI regularAi = gameObject.GetComponent<GuardAI>();
        PlayerDetection detector = transform.GetChild(0).GetComponent<PlayerDetection>();
        LineRenderer visionCone = transform.GetChild(0).GetComponent<LineRenderer>();        

        regularAi.enabled = false;
        detector.enabled = false;
        visionCone.enabled = false;

        Destroy(coin);

        while (timer < maxTime)
        {
            Debug.Log("admiring time = " + timer);
            timer+= 0.03f;
            yield return null;
        }

        regularAi.enabled = true;
        detector.enabled = true;
        visionCone.enabled = true;
    }

    
}
