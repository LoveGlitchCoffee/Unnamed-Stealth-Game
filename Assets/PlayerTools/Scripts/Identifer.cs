using UnityEngine;
using System.Collections;

public class Identifer : MonoBehaviour
{

    private Tool _identity;


    public void SetIdentity(Tool identityTool)
    {
        _identity = identityTool;
        SetImage();
    }

    private void SetImage()
    {
        
    }

    public Tool GetIdentity()
    {
        return _identity;
    }
}
