using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct GraphOfMap
{
    //change here
    public List<Node> _abstractMap;

    //diametre is 2


    public Node nodeWith(Node node)
    {
        Node _returnNode = null;
        bool nodeFound = false;
        int i = 0;

        while (nodeFound == false && i < _abstractMap.Count)
        {
            if (_abstractMap.ElementAt(i).GetX() == node.GetX() && _abstractMap.ElementAt(i).GetY() == node.GetY())
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
