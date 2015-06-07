using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public struct BFS
{

    public List<Node> _frontier;
    public HashSet<Node> _visited;
    public Dictionary<Node, Node> _possiblePath;

    /**
     * searches for a route from the start node to goal node
     */
    public Node[] FindRouteFrom(Node start, Node goal)
    {

        Node current;
        Node parent;
        
        List<Node> path = new List<Node> {start};

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
                    path.Reverse_NoHeapAlloc();
                    return path.ToArray();
                }
                else
                {             
                    _visited.Add(current);

                    for (int i = 0; i < current.GetSuccessors().Count; i ++)
                    {
                        Node successor = current.GetSuccessors().ElementAt(i);
                        
                        _frontier.Add(successor);
                    }

                    for (int j = 0; j < current.GetSuccessors().Count; j ++)
                    {
                        Node successor = current.GetSuccessors().ElementAt(j);

                        if (!(_possiblePath.ContainsKey(successor)) && !(_possiblePath.ContainsValue(successor)))
                        {
                            _possiblePath.Add(successor, current);                           
                        }
                    }                    
                }
            }
        }

        return new Node[0]; //empty list
    }

    
}

