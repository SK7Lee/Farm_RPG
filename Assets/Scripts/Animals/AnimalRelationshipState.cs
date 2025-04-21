using UnityEngine;

[System.Serializable]
public class AnimalRelationshipState : NPCRelationshipState
{
    public string animalType;
    public AnimalRelationshipState(string name, AnimalData animalType) : base (name)
    {
        this.animalType = animalType.name;
    }

    public AnimalRelationshipState(string name, AnimalData animalType, int friendshipPoints) : base(name, friendshipPoints)
    {
        this.animalType = animalType.name;
    }
    //convert the animal type to a string
    public AnimalData AnimalType()
    {
        return AnimalStats.GetAnimalTypeFromString(animalType);
    }

}
