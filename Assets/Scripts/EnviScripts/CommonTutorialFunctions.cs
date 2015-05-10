using UnityEngine;

public class CommonTutorialFunctions {

    private GameObject[] lootables;
    private Identifer[] identities;

    public GameObject GetLootables(string desiredObject)
    {
        GameObject objectToReturn = null;

        lootables = GameObject.FindGameObjectsWithTag("Lootable");
        identities = new Identifer[lootables.Length];


    	for (int i = 0; i < lootables.Length; i++)
	    {
	        identities[i] = lootables[i].GetComponent<Identifer>();

	        if (identities[i].ReturnIdentity() == desiredObject)
	            objectToReturn = identities[i].gameObject;
        }

        return objectToReturn;
    }
    
}
