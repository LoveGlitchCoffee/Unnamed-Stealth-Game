using UnityEngine;
using System.Collections;

public class RatCollecting : MonoBehaviour {

    private GameObject _ratGuard;
    private GameObject[] _guardsWithRat;
    public GameObject InteractablePrefab;
    private DetectionCommon _detection;

    /*
     * In all of the guards in the scene with a rat, or had 1
     * the guard which can collect the rat should be the one without the rat in its 'group' object
     * Case could be that this may be several guards (stole > 1 rat), in which case the first found will collect
     */
    void Start()
    {
        _guardsWithRat = GameObject.FindGameObjectsWithTag("RatGuard");
        _detection = GetComponent<DetectionCommon>();   
    }

    /*
     * Reset the node the rat is in, so it is from the owning guard's prespective, for search
     * When search complete, destroy the item and re-equip it back on to the owning guard
     */
    public IEnumerator RatGuardCollect(GameObject item, RaycastHit2D detectRat)
    {
        
        _ratGuard = FindGuardWithoutRat();        
        
        Node nodeItemIn = _detection.CalculateNodeLastSeen(detectRat);
        
        _ratGuard.GetComponent<Spritehandler>().StopAnimation("idle");
        yield return StartCoroutine(_ratGuard.GetComponent<Pathfinding>().GoToItem(nodeItemIn));
        
        Destroy(item);
        ReEquipRat();
        StartCoroutine(ResumePatrolAfterCollect());
    }

    /*
     * Duplicate code with post-pursuit, awaiting design decision
     */
    IEnumerator ResumePatrolAfterCollect()
    {        
        yield return StartCoroutine(_ratGuard.GetComponent<Pathfinding>().FinishPatrol());
        StartCoroutine(_ratGuard.GetComponent<Patrol>().Wait());
        _ratGuard.GetComponent<Pathfinding>().ResumePatrolStabaliser();

        _ratGuard.GetComponent<Patrol>().enabled = true;
        StartCoroutine(_ratGuard.GetComponent<Patrol>().Patrolling());
    }

    /*
     * Instantiates a tool with rat identity
     * reassign this to the ratguard group with apprpriate settings
     */
    private void ReEquipRat()
    {
        GameObject newRat = (GameObject) Instantiate(InteractablePrefab, _ratGuard.transform.position, new Quaternion(0,0,0,0));
        newRat.transform.parent = _ratGuard.transform.parent;
        newRat.layer = 2;        
        newRat.AddComponent<Animator>();
        newRat.GetComponent<Animator>().runtimeAnimatorController =
            Resources.Load("RatAnimation") as RuntimeAnimatorController;
        newRat.AddComponent<FollowGuard>();
        newRat.GetComponent<SpriteRenderer>().sortingLayerName = "LivingTool";
        newRat.AddComponent<PresetIdentity>();
        newRat.GetComponent<PresetIdentity>().indexInDb = 2;
        newRat.GetComponent<CircleCollider2D>().enabled = true;
    }

    /*
     * Finds the guard without a rat (who initially had one, so belongs to _guardsWithRat group)
     * index 0 as they should be the only child of the group
     * as contingency, could just the first guard without rat in group
     */
    private GameObject FindGuardWithoutRat()
    {
        for (int i = 0; i < _guardsWithRat.Length; i++)
        {
            if (_guardsWithRat[i].transform.childCount < 2)
            {
                Debug.Log(_guardsWithRat[i].transform.GetChild(0).name);
                return _guardsWithRat[i].transform.GetChild(0).gameObject;
            }
                
        }
        
        return _guardsWithRat[0].transform.GetChild(0).gameObject; 
    }
}
