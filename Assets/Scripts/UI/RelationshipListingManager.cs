using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class RelationshipListingManager : ListingManager<NPCRelationshipState>
{
    List<CharacterData> characters; // Corrected type to List<CharacterData>  

    protected override void DisplayListing(NPCRelationshipState relationship, GameObject listingGameObject)
    {
        if (characters == null) 
        { 
            LoadAllCharacters(); // Load characters only if they are null
        }
        CharacterData characterData = GetCharacterDataFromString(relationship.name);
        listingGameObject.GetComponent<NPCRelationshipListing>().Display(characterData, relationship); // Use the correct type for the character data
    }

    public CharacterData GetCharacterDataFromString(string name)
    {
        return characters.Find(i => i.name == name); // Use the correct type for the character data
    }
    void LoadAllCharacters()
    {
        characters = NPCManager.Instance.Characters(); // Load characters from NPCManager
    }
}
