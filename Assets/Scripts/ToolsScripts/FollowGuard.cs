using UnityEngine;
using System.Collections;

public class FollowGuard : MonoBehaviour
{

    private GameObject _guard;

	void Awake ()
	{
	    _guard = transform.parent.Find("Guard").gameObject;
	}
	
	
	void Update ()
	{
	    transform.position = new Vector3(_guard.transform.position.x,_guard.transform.position.y, _guard.transform.position.z - 2);
	}
}
