using UnityEngine;

[System.Serializable]
public class NPCRelationshipState 
{
    public string name;
    public int friendshipPoints;

    public bool hasTalkedToday;
    public bool giftGivenToday;
    public NPCRelationshipState(string name, int friendshipPoints)
    {
        this.name = name;
        this.friendshipPoints = friendshipPoints;
    }

    public NPCRelationshipState(string name)
    {
        this.name = name;
        friendshipPoints = 0;
    }
    public int Hearts
    {
        get { return friendshipPoints / 250; }
    }
}
