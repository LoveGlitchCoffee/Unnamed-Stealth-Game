using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BFS
{

    private List<Node> _frontier;
    private HashSet<Node> _visited;
    private Dictionary<Node, Node> _possiblePath;

    public BFS()
    {
        _frontier = new List<Node>();
        _visited = new HashSet<Node>();
        _possiblePath = new Dictionary<Node, Node>();
    }

    public List<Node> FindRouteFrom(Node start, Node goal)
    {

        Node current;
        Node parent;
        List<Node> path = new List<Node>();
        path.Add(start);

        _frontier.Clear();
        _visited.Clear();
        _possiblePath.Clear();

        _frontier.Add(start);


        while (_frontier.Count != 0)
        {


            current = _frontier.ElementAt(0);
            _frontier.RemoveAt(0);

            Debug.Log("current " + current.GetX());
            Debug.Log("current " + current.GetY());

            if (!_visited.Contains(current))
            {

                if (current.CompareTo(goal) == 0)
                {
                    while (_possiblePath.ContainsKey(current))
                    {
                        path.Add(current);

                        parent = _possiblePath[current];
                        current = parent;
                    }

                    path.RemoveAt(0);
                    path.Reverse();
                    
                }
                else
                {
                    _visited.Add(current);

                    for (int i = 0; i < current.GetSuccessors().Count; i ++)
                    {
                        Node successor = current.GetSuccessors().ElementAt(i);
                        
                        Debug.Log("x: " + successor.GetX());
                        Debug.Log("y: " + successor.GetY());
                        Debug.Log("");
                        _frontier.Add(successor);
                    }

                    for (int j = 0; j < current.GetSuccessors().Count; j ++)
                    {
                        Node successor = current.GetSuccessors().ElementAt(j);

                        if (!(_possiblePath.ContainsKey(successor)))
                        {
                            _possiblePath.Add(successor, current);                           
                        }
                    }
                    
                }
            }
        }

        return new List<Node>(); // empty list
    }
}

