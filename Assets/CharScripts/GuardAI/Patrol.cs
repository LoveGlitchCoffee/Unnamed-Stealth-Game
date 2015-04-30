using System;
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

    private bool _patrolling = true;
    
    private Node _nodeGuardAt;

    void Awake()
    {
        _playerDetection = gameObject.GetComponentInChildren<PlayerDetection>();
        _pathfinding = gameObject.GetComponent<Pathfinding>();
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<GenerateNodes>().ReturnGeneratedGraph();            
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
        
        while (_patrolling)
        {                
            yield return StartCoroutine(_pathfinding.PatrolOnRoute(patrolRoute));          
            yield return StartCoroutine(Wait());
            TurnAround();
            patrolRoute = _pathfinding.ReverseRoute(patrolRoute);
            //put checks in pathfind to interrupt patrol?
        }
                       
    }

   
    /**
     * Waits for 5 seconds and turns around according to previous walking direction, then continue patrol
     * changes vision cone direction as well
     */
    public void TurnAround()
    {
        GoingLeft = !GoingLeft;
        GetComponent<Spritehandler>().FlipSprite();
        _playerDetection.SetVisionCone(GoingLeft);
    }


    /**
     * Wait 5 seconds
     */
    public IEnumerator Wait()
    {
        float currentTime = 0f;
        float maxWaitTime = 10f;

        GetComponent<Spritehandler>().GuardIdle = true;

        while (currentTime < maxWaitTime)
        {
            currentTime += 0.1f;
            yield return null;
        }

        GetComponent<Spritehandler>().GuardIdle = false;
    }

    /*
     * for each coordinates, given in inspector, find the node in map that corresponds and assign them to the patrol route
     */
    private Node[] AssignRoute()
    {
        Node[] route = new Node[Coordinates.Count/2];
        int counter = 0;

        for (int j = 0; j < Coordinates.Count; j += 2)
        {
            for (int i = 0; i < map.AbstractMap.Count; i++)
            {
                Node node = map.AbstractMap[i];
                
                if (node.GetX() == Coordinates[j] && node.GetY() == Coordinates[j + 1])
                {
                    route[counter] = node;
                    //Debug.Log("node is " + node.GetX() + ", " + node.GetY());
                    counter++;
                }
            }       
        }
        
        return route;
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
