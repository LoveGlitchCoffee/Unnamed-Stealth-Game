using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GraphOfMap
{

    private List<Node> _abstractMap;

    //diametre is 2

    public GraphOfMap()
    {
        _abstractMap = new List<Node>();
    }

    public Node nodeWith(Node node)
    {
        Node _returnNode = null;
        bool nodeFound = false;
        int i = 0;

        while (nodeFound == false && i < _abstractMap.Count)
        {
            if (_abstractMap.ElementAt(i).transform.position.x == node.transform.position.x && _abstractMap.ElementAt(i).transform.position.y == node.transform.position.y)
            {
                _returnNode = _abstractMap.ElementAt(i);
                nodeFound = true;
            }
            else
                i++;
        }

        if (nodeFound == false)
        {
            _returnNode = node;
            _abstractMap.Add(node);
        }

        return _returnNode;
    }

    public List<Node> ReturnGraph()
    {
        return _abstractMap;
    }
}
