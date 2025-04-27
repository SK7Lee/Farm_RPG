using UnityEngine;

[System.Serializable]
public class AnimalRelationshipState : NPCRelationshipState
{
    public string animalType;
    const int MAX_MOOD = 255;
    private int _mood;

    public int age;
    public int Mood
    {
        get => _mood;
        set
        {
            _mood = Mathf.Clamp(value, 0, MAX_MOOD);
        }
    }

    public AnimalRelationshipState(string name, AnimalData animalType) : base (name)
    {
        this.animalType = animalType.name;
        Mood = MAX_MOOD; // Set default mood to max
    }

    public AnimalRelationshipState(string name, AnimalData animalType, int friendshipPoints, int mood) : base(name, friendshipPoints)
    {
        this.animalType = animalType.name;
        Mood = mood; 

    }
    //convert the animal type to a string
    public AnimalData AnimalType()
    {
        return AnimalStats.GetAnimalTypeFromString(animalType);
    }

}
