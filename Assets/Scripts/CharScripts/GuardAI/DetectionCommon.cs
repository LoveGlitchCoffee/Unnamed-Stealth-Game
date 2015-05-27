using UnityEngine;
using System.Collections;

public class DetectionCommon : MonoBehaviour {


    Ray2D _lineOfSight;
    private Patrol _patrolBehav;

    private const int LayerLiving = 11;
    private const int LayerEnvi = 10;
    private const int LayerItem = 14;
    private LayerMask _detectLiving;
    private LayerMask _detectEnvi;
    private LayerMask _detectItem;
    private LayerMask _detectLayerMask;

    private const float EyeDistance = 0.4f;
    private int _sightDistance;

    private GameObject _gameMap;
    private GraphOfMap _graph;

    void Awake()
    {
        _detectLiving = 1 << LayerLiving;
        _detectEnvi = 1 << LayerEnvi;
        _detectItem = 1 << LayerItem;

        LayerMask _detectLayerMask0 = _detectLiving | _detectEnvi;

        _detectLayerMask = _detectLayerMask0 | _detectItem;

        _sightDistance = 4;

        _patrolBehav = GetComponentInParent<Patrol>();

        _gameMap = GameObject.FindGameObjectWithTag("Map");
        _graph = _gameMap.GetComponent<NodeGenerator>().ReturnGeneratedGraph();        
    }


    public RaycastHit2D CheckIfHit(GameObject itemToCheckFor)
    {
        float direction = 0.5f;

        if (_patrolBehav.GoingLeft)
            direction *= -1;

        _lineOfSight = new Ray2D(new Vector2(gameObject.transform.position.x + direction, gameObject.transform.position.y + EyeDistance), CalculateDirection(itemToCheckFor));
        RaycastHit2D detectItem = Physics2D.Raycast(_lineOfSight.origin, _lineOfSight.direction, _sightDistance, _detectLayerMask); // distance is x distance       
        Debug.DrawLine(_lineOfSight.origin, _lineOfSight.direction);

        return detectItem;
    }

    
    /**
     * Calculates the direction to cast ray, should be direction towards player
     */
    private Vector2 CalculateDirection(GameObject itemGameObject)
    {
        return new Vector2(itemGameObject.transform.position.x - gameObject.transform.position.x, itemGameObject.transform.position.y - gameObject.transform.position.y);
    }

    public float GetEyeDistance()
    {
        return EyeDistance;
    }

    

    /*
     * Calculates the node player last seen (checks which collider overlaps point of last seen
     * if last seen is outside collider, calculate closest point
     */
    public Node CalculateNodeLastSeen(RaycastHit2D itemLastSeen, GameObject guard)
    {
        Vector2 pointLastSeen = itemLastSeen.point;
        Node alternateNode = null;
        bool nodeLower;

        if (guard == null)
            nodeLower = _patrolBehav.ReturnNodeGuardAt().GetY() > pointLastSeen.y;
        else
        {
            nodeLower = guard.GetComponent<Patrol>().ReturnNodeGuardAt().GetY() > pointLastSeen.y;
        }


        Debug.Log("point last seen " + pointLastSeen);        

        alternateNode = CheckIfOverlapPoint(pointLastSeen, alternateNode);        

        if (alternateNode == null)
        {            

            if (_patrolBehav.GoingLeft)
            {
                for (int i = 0; i < _gameMap.transform.childCount; i++)
                {
                    CircleCollider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();

                    if (nodeCollider != null)
                    {
                        Vector2 nodePosition = nodeCollider.transform.position;

                        if (!(nodePosition.x > pointLastSeen.x))
                            if (nodeLower)
                            {
                                Debug.Log("node is lower");
                                if (nodePosition.y <= pointLastSeen.y)
                                    alternateNode = nodeCollider.gameObject.GetComponent<Node>();    
                            }
                            else
                            {
                                Debug.Log("node is higher");
                                if (nodePosition.y >= pointLastSeen.y)
                                    alternateNode = nodeCollider.gameObject.GetComponent<Node>(); 
                            }
                            
                    }
                }
            }
            else if (!_patrolBehav.GoingLeft)
            {

                for (int i = _gameMap.transform.childCount - 1; i > -1; i--)
                {
                    CircleCollider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();

                    if (nodeCollider != null)
                    {
                        Vector2 nodePosition = nodeCollider.transform.position;
                        
                        if (!(nodePosition.x < pointLastSeen.x))
                            if (nodeLower)
                            {
                                Debug.Log("node is lower");
                                if (nodePosition.y <= pointLastSeen.y)
                                    alternateNode = nodeCollider.gameObject.GetComponent<Node>();
                            }
                            else
                            {
                                Debug.Log("node is higher");
                                if (nodePosition.y >= pointLastSeen.y)
                                    alternateNode = nodeCollider.gameObject.GetComponent<Node>();
                            }
                    }
                }

            }
        }        

        Debug.Log(alternateNode.GetX() +", "+ alternateNode.GetY());
        return alternateNode;
    }

    private Node CheckIfOverlapPoint(Vector2 pointLastSeen, Node alternate)
    {
        for (int i = 0; i < _gameMap.transform.childCount; i++)
        {
            Collider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();

            if (nodeCollider != null)
            {                
                if (nodeCollider.OverlapPoint(pointLastSeen))                    
                    return _graph.nodeWith(nodeCollider.gameObject.GetComponent<Node>());
            }
        }

        return alternate;
    }
}
