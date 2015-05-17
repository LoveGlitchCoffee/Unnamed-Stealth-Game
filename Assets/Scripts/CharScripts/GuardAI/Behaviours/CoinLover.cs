using UnityEngine;
using System.Collections;

public class CoinLover : MonoBehaviour, IBehaviour
{
    private const string lootTag = "Lootable";
    private const string coinName = "coin";
    private Pathfinding _pathFinding;
    private Patrol _patrolBehav;
    private GuardSoundHandler _soundHandler;
    private PlayerDetection _detector;
    private LineRenderer _visionCone;

    void Awake()
    {
        _pathFinding = GetComponent<Pathfinding>();
        _patrolBehav = GetComponent<Patrol>();
        _soundHandler = GetComponent<GuardSoundHandler>();
         _detector = transform.GetChild(0).GetComponent<PlayerDetection>();
        _visionCone = transform.GetChild(0).GetComponent<LineRenderer>();
    }

    /*
     * If guard comes in contact with a coin on the ground. He stops all movement and detection
     */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == lootTag && col.GetComponent<Identifer>().ReturnIdentity() == coinName)
        {       
            _pathFinding.StopAllCoroutines();
            _detector.StopAllCoroutines();            
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
                
        _patrolBehav.enabled = false;
        _detector.enabled = false;
        _visionCone.enabled = false;

        Debug.Log("got coin");
        Destroy(coin);
        _soundHandler.PlaySound("Confused", 0.5f);

        while (timer < maxTime)
        {            
            timer+= 0.03f;
            yield return null;
        }

        _visionCone.enabled = true;
        _detector.enabled = true;        
        yield return StartCoroutine(_pathFinding.FinishPatrol());                
        yield return StartCoroutine(_patrolBehav.Wait());
        _pathFinding.ResumePatrolStabaliser();        
        _patrolBehav.enabled = true;       
        StartCoroutine(_patrolBehav.Patrolling());         
    }


    public string ReturnBehaviourDescription()
    {
        return "Greedy and love coins, if he sees one, he'll forget the world just to look at it";
    }
}
