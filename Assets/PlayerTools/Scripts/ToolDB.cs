using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDB {

    public List<Tool> ToolDatabase = new List<Tool>();

    public ToolDB()
    {
        ToolDatabase.Add(new Tool(0, "key", Tool.ItemType.key, "A key, must open a door somewhere"));
        ToolDatabase.Add(new Tool(1, "coin", Tool.ItemType.distraction, "coins for trade"));
    }

}
