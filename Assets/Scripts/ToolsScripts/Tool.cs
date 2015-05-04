using UnityEngine;

public class Tool
{

    public int ItemId;
    public string Name;
    public Sprite ItemImage;
    public ItemType itemType;
    public string ItemDescription;

    public enum ItemType
    {
        distraction,
        trap,
        key
    }

   public Tool(int id, string name, ItemType itemType, string description)
	{
		this.ItemId = id;
		this.Name = name;
		this.itemType = itemType;
		this.ItemImage = Resources.Load<Sprite>(""+name);
       this.ItemDescription = description;
	}

	public Tool()
	{

	}
}