using UnityEngine;
using System.Collections;

public class DrawBoundaries : MonoBehaviour
{

    public GameObject _texture;
    public int Length;
    public int Height;
    public float XDistance;
    public float YDistance;
    public int StartX;
    private int _currentX;
    public int CurrentY;

    /*
     * Draws ground
     */
	void Start ()
	{
	    _currentX = StartX;

	    for (int i = 0; i < Height; i++)
	    {
	        for (int j = 0; j < Length; j++)
	        {
	            GameObject newTile = (GameObject) Instantiate(_texture, new Vector3(_currentX*XDistance, CurrentY*YDistance, 1), _texture.transform.rotation);
	            newTile.transform.parent = transform;                
	            newTile.transform.localScale = new Vector3(0.2f,1,1);
	            _currentX++;
	        }
	        _currentX = StartX;
	        CurrentY++;
	    }
	}
	
	
}
