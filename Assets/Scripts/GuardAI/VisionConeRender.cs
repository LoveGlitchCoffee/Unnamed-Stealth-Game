using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VisionConeRender : MonoBehaviour
{

    private LineRenderer _visionCone;

 
    private Color _far = Color.clear;
   

	// Use this for initialization
	void Start ()
	{
	    _visionCone = GetComponent<LineRenderer>();
	    _far.a = 0.45f;

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    /*
     * Sets the vision cone's near and far position and width according to abstract player detection
     * current width is being set continuosly, refactor to set once
     */
    public void SetConeShape(Vector2[] visionField)
    {
        Vector2 nearUp = visionField[0];
        Vector2 farUp = visionField[1];
        Vector2 farDown = visionField[2];
        Vector2 nearDown = visionField[3];        
        
        _visionCone.SetPosition(0, new Vector3(nearUp.x, nearUp.y - (nearUp.y - nearDown.y)/2));
        _visionCone.SetPosition(1, new Vector3(farUp.x,farUp.y - (farUp.y - farDown.y)/2));        

        _visionCone.SetWidth(nearUp.y - nearDown.y,farUp.y - farDown.y );
    }    

    public void ActivateState(Color state)
    {
        _visionCone.SetColors(state,_far);
    }


}
