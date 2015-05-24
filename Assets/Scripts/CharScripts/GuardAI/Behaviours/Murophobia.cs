using UnityEngine;
using System.Collections;

public class Murophobia : MonoBehaviour{

    private const string lootTag = "Lootable";
    private const string ratName = "rat";
    private Pathfinding _pathFinding;
    private Patrol _patrolBehav;
    private GuardSoundHandler _soundHandler;
    private Spritehandler _spriteHandler;
    private PlayerDetection _detector;
    private LineRenderer _visionCone;

    void Awake()
    {
        _pathFinding = GetComponentInParent<Pathfinding>();
        _patrolBehav = GetComponentInParent<Patrol>();
        _soundHandler = GetComponentInParent<GuardSoundHandler>();
        _spriteHandler = GetComponentInParent<Spritehandler>();
        _detector = GetComponent<PlayerDetection>();
        _visionCone = GetComponent<LineRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {        

        if (col.gameObject.tag == lootTag && col.GetComponent<Identifer>().ReturnIdentity() == ratName)
        {
            _patrolBehav.StopAllCoroutines();
            _pathFinding.StopAllCoroutines();
            _detector.StopAllCoroutines();
            StartCoroutine(PhobiaReaction());
        }
    }
    

    /*
     * All movement and detection is disabled for a set period of time
     */
    IEnumerator PhobiaReaction()
    {
        _patrolBehav.enabled = false;
        _detector.enabled = false;
        _visionCone.enabled = false;

        const float realiseTime = 3f;
        float timer = 0f;
        
        while (timer < realiseTime)
        {
            timer += 1f;            
            yield return null;
        }
      
        _soundHandler.PlaySound("Confused", 0.5f);        
    }


    void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.tag == lootTag && col.GetComponent<Identifer>().ReturnIdentity() == ratName)
        {
            StartCoroutine(OffPhobia());            
        }
    }

    private IEnumerator OffPhobia()
    {
        _visionCone.enabled = true;
        _detector.enabled = true;
        yield return StartCoroutine(_pathFinding.FinishPatrol());
        yield return StartCoroutine(_patrolBehav.Wait());
        _pathFinding.ResumePatrolStabaliser();
        _patrolBehav.enabled = true;
        StartCoroutine(_patrolBehav.Patrolling());
    }
       
}
