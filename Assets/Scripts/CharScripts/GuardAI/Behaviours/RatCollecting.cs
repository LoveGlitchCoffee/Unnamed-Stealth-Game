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

    public IEnumerator RatGuardCollect(GameObject item, RaycastHit2D detectRat)
    {
        _ratGuard = FindGuardWithoutRat();
        Debug.Log(_ratGuard.name);
        Node nodeItemIn = _detection.CalculateNodeLastSeen(detectRat, _ratGuard);

        Debug.Log("node is " + nodeItemIn.GetX() + ", " + nodeItemIn.GetY());
        
        yield return StartCoroutine(_ratGuard.GetComponent<Pathfinding>().GoToItem(nodeItemIn));
        Debug.Log("found rat");
        Destroy(item);
        ReEquipRat();        
    }

    private void ReEquipRat()
    {
        GameObject newRat = Instantiate(InteractablePrefab);
        newRat.transform.parent = _ratGuard.transform.parent;
        newRat.layer = 2;
        //may have to do sorting layer
        newRat.AddComponent<Animator>();
        newRat.GetComponent<Animator>().runtimeAnimatorController =
            Resources.Load("RatAnimation") as RuntimeAnimatorController;
        newRat.AddComponent<FollowGuard>();
        newRat.AddComponent<PresetIdentity>();
        newRat.GetComponent<PresetIdentity>().indexInDb = 2;

        

    }

    private GameObject FindGuardWithoutRat()
    {
        for (int i = 0; i < _guardsWithRat.Length; i++)
        {
            if (_guardsWithRat[i].transform.childCount < 1)
            {
                Debug.Log(_guardsWithRat[i].transform.GetChild(0).name);
                return _guardsWithRat[i].transform.GetChild(0).gameObject;
            }
                
        }

        //testing, should be 2
        return _guardsWithRat[0].transform.GetChild(1).gameObject; // should never reach here, but as contingency
    }
}
