using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Patrol : MonoBehaviour {

    [HideInInspector] public bool GoingLeft;
    public List<float> Coordinates;

    private PlayerDetection _playerDetection;
    private Pathfinding _pathfinding;

    private GraphOfMap map;

    private const float WalkSpeed = 1f;
    private float _waitTime = 0;
    private const float MaxWaitTime = 2f;
    
    private Node _nodeGuardAt;

    void Awake()
    {
        _playerDetection = gameObject.GetComponentInChildren<PlayerDetection>();
        _pathfinding = gameObject.GetComponent<Pathfinding>();
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<GenerateNodes>().ReturnGeneratedGraph();    
        Coordinates = new List<float>();
    }

    /**
     * Guard Patrol along scripted route
     * Always in patrol region going left     
     */
    private void Start()
    {        
        GoingLeft = false;

        StartCoroutine(Patrolling());
    }

    IEnumerator Patrolling()
    {
        Node[] patrolRoute = AssignRoute();
        
        _pathfinding.PatrolOnRoute(patrolRoute);
        _pathfinding.ReverseRoute();
        Debug.Log("route = " + patrolRoute);

        yield return null;
    }

    private Node[] AssignRoute()
    {
        Node[] route = new Node[Coordinates.Count];
        int counter = 0;

        for (int j = 0; j < Coordinates.Count; j += 2)
        {
            for (int i = 0; i < map.AbstractMap.Count; i++)
            {
                Node node = map.AbstractMap[i];
                
                if (node.GetX() == Coordinates[j] && node.GetY() == Coordinates[j + 1])
                {
                    route[counter] = node;
                    counter++;
                }
            }       
        }
        
        return route;
    }


    /**
     * Waits for 5 seconds and turns around according to previous walking direction, then continue patrol
     * changes vision cone direction as well
     */
    private void TurnAround()
    {
        Wait();

        if (_waitTime >= MaxWaitTime)
        {
            GoingLeft = !GoingLeft;
            GetComponent<Spritehandler>().FlipSprite();
            _waitTime = 0f;
            //OutOfPatrolArea = false;
            _playerDetection.SetVisionCone(GoingLeft);
        }
    }


    /**
     * Wait 5 seconds
     */
    private void Wait()
    {
        if (_waitTime < MaxWaitTime)
        {
            _waitTime += 1f * Time.deltaTime;
        }
    }
   
    
    /**
     * tracks which map node guard is currently at
     */
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {            
            _nodeGuardAt =
                GameObject.FindGameObjectWithTag("Map")
                    .GetComponent<GenerateNodes>()
                    .ReturnGeneratedGraph()
                    .nodeWith(col.GetComponent<Node>());

            //Debug.Log(_nodeGuardAt.GetX() + ", " + _nodeGuardAt.GetY());
        }
    }

    /**
     * returns node on map guard is at at time of method call
     */
    public Node ReturnNodeGuardAt()
    {        
        return _nodeGuardAt;
    }
   

}
