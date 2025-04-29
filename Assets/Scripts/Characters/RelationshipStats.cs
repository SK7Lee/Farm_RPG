using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStats : MonoBehaviour
{
    const string RELATIONSHIP_PREFIX = "NPCRelationship_";
    public static List<NPCRelationshipState> relationships = new List<NPCRelationshipState>();
    public enum GiftReaction
    {
        Neutral,
        Like,
        Dislike
    }
    public static void LoadStats()
    {
        relationships = new List<NPCRelationshipState>();
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();

        foreach (CharacterData c in NPCManager.Instance.Characters())
        {
            string key = RELATIONSHIP_PREFIX + c.name;
            if (blackboard.ContainsKey(key) && blackboard.TryGetValue(key, out NPCRelationshipState rs))
            {
                relationships.Add(rs);
            }
            else
            {
                Debug.LogWarning($"Relationship data for {c.name} not found in the blackboard.");
            }
        }

        /*
        if (relationshipsToLoad == null)
        {
            relationships = new List<NPCRelationshipState>();
            return;
        }
        relationships = relationshipsToLoad;
        */
    }
    //check if the character is already in the list of relationships
    public static bool FirstMeeting(CharacterData character)
    {
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();
        return !blackboard.ContainsKey(RELATIONSHIP_PREFIX + character.name);
        //return !relationships.Exists(i=>i.name == character.name);
    }

    //Get relationship state of a character
    public static NPCRelationshipState GetRelationship(CharacterData character)
    {
        //check if it is the first meeting of the day
        if (FirstMeeting(character))
            return null;

        var blackboard = GameStateManager.Instance.GetBlackboard();
        if (blackboard.TryGetValue(RELATIONSHIP_PREFIX + character.name, out NPCRelationshipState rs))
            return rs;

        return null;

        //return relationships.Find(i => i.name == character.name);
    }
    //Add character to the list of relationships
    public static void UnlockCharacter(CharacterData character)
    {
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();
        NPCRelationshipState relationship = new NPCRelationshipState(character.name);
        blackboard.SetValue(RELATIONSHIP_PREFIX + character.name, relationship);
        relationships.Add(relationship);
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

    public static bool GiftGivenToday(CharacterData character)
    {
        NPCRelationshipState npc = GetRelationship(character);
        return npc.giftGivenToday;
    }

    public static GiftReaction GetReactionToGift(CharacterData character, ItemData item)
    {
        //if (FirstMeeting(character)) return GiftReaction.Neutral;
        if (character.likes.Contains(item))
        {
            return GiftReaction.Like;
        }
        else if (character.dislikes.Contains(item))
        {
            return GiftReaction.Dislike;
        }
        return GiftReaction.Neutral;
    }
    
    public static bool IsBirthday(CharacterData character)
    {
        GameTimestamp birthday = character.birthday;
        GameTimestamp today = TimeManager.Instance.GetGameTimestamp();
        return (today.day == birthday.day) && (today.season == birthday.season);
    }

    public static bool IsBirthday(CharacterData character, GameTimestamp today)
    {
        GameTimestamp birthday = character.birthday;
        return (today.day == birthday.day) && (today.season == birthday.season);
    } 
    public static CharacterData WhoseBirthday(GameTimestamp timestamp)
    {
        foreach (CharacterData character in NPCManager.Instance.Characters())
        {
            if (IsBirthday(character, timestamp))
            {
                return character;
            }
        }
        return null;
    }
}
