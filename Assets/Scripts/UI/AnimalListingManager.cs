using UnityEngine;

public class AnimalListingManager : ListingManager<AnimalRelationshipState>
{
    protected override void DisplayListing(AnimalRelationshipState relationship, GameObject listingGameObject)
    {
        //Get the AnimalData from the string in the relationship
        AnimalData animalData = AnimalStats.GetAnimalTypeFromString(relationship.animalType);

        listingGameObject.GetComponent<NPCRelationshipListing>().Display(animalData, relationship);
    }

   
}
