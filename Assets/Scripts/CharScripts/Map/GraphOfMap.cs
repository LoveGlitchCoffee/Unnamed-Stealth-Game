using System.Collections.Generic;
using System.Linq;

public struct GraphOfMap
{
    // A list of nodes that represents the graph of the map
    public List<Node> AbstractMap;

    /*
     * chekcs to see if the passed node is in the map
     * if it is, return that node, otherwise creates a new one and return that instead
     */
    public Node nodeWith(Node node)
    {
        Node _returnNode = null;
        bool nodeFound = false;
        int i = 0;

        while (nodeFound == false && i < AbstractMap.Count)
        {
            if (AbstractMap.ElementAt(i).GetX() == node.GetX() && AbstractMap.ElementAt(i).GetY() == node.GetY())
            {
                _returnNode = AbstractMap.ElementAt(i);
                nodeFound = true;
            }
            else
                i++;
        }

        if (nodeFound == false)
        {
            _returnNode = node;
            AbstractMap.Add(node);
        }

        return _returnNode;
    }

    /**
     * return the graph 
     */
    public List<Node> ReturnGraph()
    {
        return AbstractMap;
    }
}
