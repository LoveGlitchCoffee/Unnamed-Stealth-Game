﻿using UnityEngine;

public class CameraLerp : MonoBehaviour
{

    private Transform _camerasPosition;
    private GameObject _cameraLead;

    void Awake()
    {
        _camerasPosition = gameObject.transform;
        _cameraLead = GameObject.FindGameObjectWithTag("CameraLead");
    }
		
	void Update ()
	{
	    _camerasPosition.position = Vector3.Lerp(_camerasPosition.position, _cameraLead.transform.position, Time.deltaTime*4f);
	}
}