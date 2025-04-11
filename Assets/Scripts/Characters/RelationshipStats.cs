using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStats : MonoBehaviour
{
    public static List<NPCRelationshipState> relationships = new List<NPCRelationshipState>();

    public static void LoadStats(List<NPCRelationshipState> relationshipsToLoad)
    {
        if (relationshipsToLoad == null)
        {
            relationships = new List<NPCRelationshipState>();
            return;
        }
        relationships = relationshipsToLoad;
    }
    //check if the character is already in the list of relationships
    public static bool FirstMeeting(CharacterData character)
    {
        return !relationships.Exists(i=>i.name == character.name);
    }
    //Get relationship state of a character
    public static NPCRelationshipState GetRelationship(CharacterData character)
    {
        if (FirstMeeting(character)) return null;
        return relationships.Find(i => i.name == character.name);
    }
    //Add character to the list of relationships
    public static void UnlockCharacter(CharacterData character)
    {
        relationships.Add(new NPCRelationshipState(character.name));
    }
    //Add friendship points to a character
    public static void AddFriendshipPoints(CharacterData character, int points)
    {
        if (FirstMeeting(character))
        {
            Debug.LogError("The player has not met this character yet!");
            return;
        }
        GetRelationship(character).friendshipPoints += points;
    }

    public static bool IsFirstConversationOfTheDay(CharacterData character)
    {
        if (FirstMeeting(character)) return true;
        NPCRelationshipState npc = GetRelationship(character);
        return !npc.hasTalkedToday;
    }
}
