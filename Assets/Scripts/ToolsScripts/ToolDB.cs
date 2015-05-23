using System.Collections.Generic;

public class ToolDB {

    public List<Tool> ToolDatabase = new List<Tool>();

    public ToolDB()
    {
        ToolDatabase.Add(new Tool(0, "key", Tool.ItemType.key, "A key, must open a door somewhere"));
        ToolDatabase.Add(new Tool(1, "coin", Tool.ItemType.distraction, "A coin, for you this can only buy time"));
        ToolDatabase.Add(new Tool(2, "rat",Tool.ItemType.distraction,"This creature is harmless, unless you're afarid of rodents"));
    }

}
