using UnityEngine;
using System.Collections;

public class RatCollecting : MonoBehaviour {

    private GameObject _ratGuard;
    private GameObject[] _guardsWithRat;
    public GameObject InteractablePrefab;

    /*
     * In all of the guards in the scene with a rat, or had 1
     * the guard which can collect the rat should be the one without the rat in its 'group' object
     * Case could be that this may be several guards (stole > 1 rat), in which case the first found will collect
     */
    void Start()
    {
        _guardsWithRat = GameObject.FindGameObjectsWithTag("RatGuard");    
             
    }

    public IEnumerator RatGuardCollect(GameObject item, Node nodeItemIn)
    {
        _ratGuard = FindGuardWithoutRat();

        yield return _ratGuard.GetComponent<Pathfinding>().GoToItem(nodeItemIn);

        Destroy(item);
        ReEquipRat();
    }

    private void ReEquipRat()
    {
        InteractablePrefab.layer = 9;
        //may have to do sorting layer
        InteractablePrefab.AddComponent<Animator>();
        InteractablePrefab.GetComponent<Animator>().runtimeAnimatorController =
            Resources.Load("RatAnimation") as RuntimeAnimatorController;
        InteractablePrefab.AddComponent<FollowGuard>();
        InteractablePrefab.AddComponent<PresetIdentity>();
        InteractablePrefab.GetComponent<PresetIdentity>().indexInDb = 2;

        InteractablePrefab.transform.parent = _ratGuard.transform.parent;

    }

    private GameObject FindGuardWithoutRat()
    {
        for (int i = 0; i < _guardsWithRat.Length; i++)
        {
            if (_guardsWithRat[i].transform.childCount < 2)
                return _guardsWithRat[i];
        }

        return _guardsWithRat[0]; // should never reach here, but as contingency
    }
}
