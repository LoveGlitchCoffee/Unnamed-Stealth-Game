using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BFS
{

    private Stack<Node> _frontier;
    private HashSet<Node> _visited;
    private Dictionary<Node, Node> _possiblePath;

    public BFS()
    {
        _frontier = new Stack<Node>();
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

        _frontier.Push(start);


        while (_frontier.Count != 0)
        {


            current = _frontier.Pop();

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

                    foreach (Node successor in current.GetSuccessors())
                    {
                        Debug.Log("x: " + successor.GetX());
                        Debug.Log("y: " + successor.GetY());
                        Debug.Log("");
                        _frontier.Push(successor);
                    }

                    foreach (Node successor in current.GetSuccessors())
                    {
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

