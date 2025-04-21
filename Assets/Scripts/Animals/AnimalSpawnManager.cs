using UnityEngine;

public class AnimalSpawnManager : MonoBehaviour
{
    [SerializeField] Collider floor;
    void Start()
    {
        RenderAnimals();
    }

    public void RenderAnimals()
    {
        foreach (AnimalRelationshipState animalRelation in AnimalStats.animalRelationships)
        {
            AnimalData animalType = animalRelation.AnimalType();
            if (animalType.locationToSpawn == SceneTransitionManager.Instance.currentLocation) 
            {
                //Spawn the animal
                Bounds bounds = floor.bounds;
                float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
                float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

                Vector3 spawnPt = new Vector3(offsetX, floor.transform.position.y, offsetZ);
                float randomYRotation = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);

                AnimalBehaviour animal = Instantiate(animalType.animalObject, spawnPt, randomRotation);
                //Load the relationship
                animal.LoadRelationship(animalRelation);
            }
        }
    }

}

