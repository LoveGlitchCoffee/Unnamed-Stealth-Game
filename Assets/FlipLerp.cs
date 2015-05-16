using System;
using UnityEngine;
using System.Collections;

public class FlipLerp : MonoBehaviour
{

    private const float XDistance = 3.5f;
    private const float YDistance = 1f;
    private float _leadDistanceX;
    private float _leadDistanceY;

    public IEnumerator LerpLead(float direction)
    {        
        _leadDistanceX = transform.parent.position.x + XDistance*direction;
        _leadDistanceY = transform.parent.position.y + YDistance;

        //Debug.Log(_leadDistanceX);
        //Debug.Log(transform.position.x);
        Vector3 _globalOfLead = transform.TransformPoint(transform.position);
        Debug.Log(_globalOfLead.x);
        Debug.Log(_leadDistanceX);

        while (!(_globalOfLead.x == _leadDistanceX))
        {          
            Debug.Log("lerp");
            transform.position = Vector3.Lerp(transform.position, new Vector3(_leadDistanceX,_leadDistanceY), Time.deltaTime/2);
            yield return null;
        }
    }
}
