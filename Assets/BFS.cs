using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
                    Debug.Log(path);
                }
                else
                {
                    _visited.Add(current);
                    _frontier.AddRange(current.GetSuccessors());

                    foreach (Node successor in current.GetSuccessors())
                    {
                        if (!(_possiblePath.ContainsKey(successor)) && !(_possiblePath[successor]))
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

