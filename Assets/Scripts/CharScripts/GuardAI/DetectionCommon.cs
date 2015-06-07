using UnityEngine;

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

    /*
     * Ray casts towards the item that is passed
     * returns information about the 'hit', if there is one
     * direction is determined by guard's orientation
     */
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
     * Calculates the direction to cast ray, should be direction towards given object
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
     * NOTE: depending on guard's orientation, node is checked in ascending or descending order of index
     */
    public Node CalculateNodeLastSeen(RaycastHit2D itemLastSeen)
    {
        Vector2 pointLastSeen = itemLastSeen.point;
        Node alternateNode = null;               
        
        alternateNode = CheckIfOverlapPoint(pointLastSeen, alternateNode);

        if (alternateNode != null)
            return alternateNode;
        

        if (_patrolBehav.GoingLeft)
        {
            for (int i = 0; i < _gameMap.transform.childCount; i++)
            {
                CircleCollider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();
                alternateNode = FindAlternateNode(nodeCollider, pointLastSeen, alternateNode, _patrolBehav.GoingLeft);
            }
        }


        else if (!_patrolBehav.GoingLeft)
        {
            for (int i = _gameMap.transform.childCount - 1; i > -1; i--)
            {
                CircleCollider2D nodeCollider = _gameMap.transform.GetChild(i).GetComponent<CircleCollider2D>();
                alternateNode = FindAlternateNode(nodeCollider, pointLastSeen, alternateNode, _patrolBehav.GoingLeft);
            }
        }

        //Debug.Log(alternateNode.GetX() +", "+ alternateNode.GetY());
        return alternateNode;
    }

    /*
     * Used if point last seen does not overlap with any node on map
     * Depending on whether guard is heading left or right, check x coordinates of each node accordingly
     * So long as y position of node is less than that of point last seen (add threshold to prevent lower than lowest y of 1.5),
     * coordinates are valid
     */
    private static Node FindAlternateNode(CircleCollider2D nodeCollider, Vector2 pointLastSeen,
        Node alternateNode, bool goingLeft)
    {
        const double threshold = 1f;

        if (nodeCollider != null)
        {
            Vector2 nodePosition = nodeCollider.transform.position;

            if (goingLeft)
            {
                
                if ((nodePosition.x < pointLastSeen.x) && (nodePosition.y <= (pointLastSeen.y + threshold)))
                {
                    alternateNode = nodeCollider.transform.gameObject.GetComponent<Node>();
                }
            }

            else
            {
                if ((nodePosition.x > pointLastSeen.x) && (nodePosition.y <= (pointLastSeen.y + threshold)))
                {
                    alternateNode = nodeCollider.transform.gameObject.GetComponent<Node>();
                }
            }
        }
        
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
