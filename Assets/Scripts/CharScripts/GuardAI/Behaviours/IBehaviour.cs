using UnityEngine;
using System.Collections;

/*
 * Unique behaviours for each guard
 * guard reacts differently to player tools depending on these behaviours
 */
public abstract class IBehaviour : MonoBehaviour
{

    protected Pathfinding _pathFinding;
    protected Patrol _patrolBehav;
    protected GuardSoundHandler _soundHandler;
    protected Spritehandler _spriteHandler;
    protected PlayerDetection _playerDetector;
    protected LineRenderer _visionCone;
    private DetectionCommon _detection;

    protected const string LootTag = "Lootable";
    public string WeaknessItem;

    protected void AssignComponents()
    {
        _pathFinding = GetComponentInParent<Pathfinding>();
        _patrolBehav = GetComponentInParent<Patrol>();
        _soundHandler = GetComponentInParent<GuardSoundHandler>();
        _spriteHandler = GetComponentInParent<Spritehandler>();
        _playerDetector = GetComponent<PlayerDetection>();
        _visionCone = GetComponent<LineRenderer>();
        _detection = GetComponent<DetectionCommon>();
    }

    /*
    * If guard comes in contact with the item they have a 'weakness' towards
     * Raycast to check if they can see it
     * If they can see it, activate their corresponding behaviour
     * NOTE: the node item is in may or may not be used
    */
    public void ReactToWeakness(Collider2D col)
    {
        if (col.tag == LootTag && col.GetComponent<Identifer>().ReturnIdentity() == WeaknessItem)
        {
            RaycastHit2D detectItem = _detection.CheckIfHit(col.gameObject);
            Node nodeItemIn = _detection.CalculateNodeLastSeen(detectItem);            

            if (detectItem.collider != null && detectItem.collider.tag == LootTag)
            {
                StopExistingCoroutines();
                StartCoroutine(ActivateBehaviour(col.gameObject, nodeItemIn));    
            }            
        }
    }

    /*
     * Stop existing coroutines, detecting and moving around,
     * to activate guard's behaviour
     */
    public void StopExistingCoroutines()
    {
        _pathFinding.StopAllCoroutines();
        _playerDetector.StopAllCoroutines();
        
        _patrolBehav.enabled = false;
        _playerDetector.enabled = false;
        this.enabled = false; //should stop continuos detection, doesn't seem to be
        _visionCone.enabled = false;
    }

    /*
     * Resume patrolling and re-activate detectors
     */
    protected IEnumerator ResumeCoroutines()
    {
        _visionCone.enabled = true;
        _playerDetector.enabled = true;
        this.enabled = true;

        yield return StartCoroutine(_pathFinding.FinishPatrol());
        yield return StartCoroutine(_patrolBehav.Wait());
        _pathFinding.ResumePatrolStabaliser();

        _patrolBehav.enabled = true;
        StartCoroutine(_patrolBehav.Patrolling());
    }

    protected abstract IEnumerator ActivateBehaviour(GameObject item, Node nodeItemIn);
    public abstract string ReturnBehaviourDescription();

    

}
