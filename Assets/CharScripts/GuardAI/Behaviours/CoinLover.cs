using UnityEngine;
using System.Collections;

public class CoinLover : MonoBehaviour, IBehaviour
{
    private const string lootTag = "Lootable";
    private const string coinName = "coin";
    private Pathfinding _pathFinding;
    private Patrol _patrolBehav;

    void Awake()
    {
        _pathFinding = GetComponent<Pathfinding>();
        _patrolBehav = GetComponent<Patrol>();
    }

    /*
     * If guard comes in contact with a coin on the ground. He stops all movement and detection
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == lootTag && col.GetComponent<Identifer>().ReturnIdentity() == coinName)
        {       
            _pathFinding.StopAllCoroutines();
            StartCoroutine(AdmireCoin(col.gameObject));
        }
    }

    /*
     * All movement and detection is disabled for a set period of time
     */
    IEnumerator AdmireCoin(GameObject coin)
    {
        const float maxTime = 7f;
        float timer = 0f;

        //could shorten if put them with interface
        Patrol regularAi = gameObject.GetComponent<Patrol>();
        PlayerDetection detector = transform.GetChild(0).GetComponent<PlayerDetection>();
        LineRenderer visionCone = transform.GetChild(0).GetComponent<LineRenderer>();        

        regularAi.enabled = false;
        detector.enabled = false;
        visionCone.enabled = false;

        Destroy(coin);

        while (timer < maxTime)
        {            
            timer+= 0.03f;
            yield return null;
        }

        yield return StartCoroutine(_pathFinding.FinishPatrol());
        yield return StartCoroutine(_patrolBehav.Wait());
        _pathFinding.ResumePatrolStabaliser();

        regularAi.enabled = true;       
        StartCoroutine(_patrolBehav.Patrolling());
        detector.enabled = true;
        visionCone.enabled = true;
    }


    public string ReturnBehaviourDescription()
    {
        return "Greedy and love coins, if he sees one, he'll forget the world just to look at it";
    }
}
