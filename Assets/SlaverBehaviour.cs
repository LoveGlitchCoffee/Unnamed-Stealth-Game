using UnityEngine;
using System.Collections;

public class SlaverBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Slave")
        {
            Debug.Log("contact");
        }
    }
}
