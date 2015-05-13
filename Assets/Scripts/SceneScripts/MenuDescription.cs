using UnityEngine;
using System.Collections;

public class MenuDescription : MonoBehaviour {

    private DescriptionWriter _writer;

    void Awake()
    {
        _writer = GetComponentInParent<DescriptionWriter>();
    }

	void Start ()
	{
	    StartCoroutine(_writer.WriteNarration("It is time, Bannerman."));
	}
	
	
}
