using UnityEngine;

public class VisionConeRender : MonoBehaviour
{

    private LineRenderer _visionCone;
 
    private Color _far = Color.clear;

    /*
     * Manipulates the Line Renderer compoenent to achive visual vision cone
     */
	void Start ()
	{
	    _visionCone = GetComponent<LineRenderer>();
	    _far.a = 0.45f;
	}
	

    /*
     * Sets the vision cone's near and far position and width according to abstract player detection
     * current width is being set continuosly
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

    /*
     * Changes the colour of the vision cone to indicate a different state
     */
    public void ActivateState(Color state)
    {
        _visionCone.SetColors(state,_far);
    }


}
