using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameSaveState 
{
    //Farm Data
    public FarmSaveState farmSaveState;

    //Inventory Data
    public InventorySaveState inventorySaveState;
    //Time Data
    public GameTimestamp timestamp;
    //Currency
    //public int money;

    public PlayerSaveState playerSaveState;

    public WeatherSaveState weatherSaveState;
    ////Relationships
    //public List<NPCRelationshipState> relationships;
    //public List<AnimalRelationshipState> animals;

    ////Animals
    //public List<EggIncubationSaveState> eggsIncubating;

    public RelationshipSaveState relationshipSaveState;
    public GameBlackboard blackboard;
    public float musicVolume = 0.5f;
    public float sfxVolume = 1.0f;
    public int currentMusicIndex = -1;
    /*
    public GameSaveState(

        FarmSaveState farmSaveState,
        InventorySaveState inventorySaveState,
        GameTimestamp timestamp,
        //int money, 
        PlayerSaveState playerSaveState,
        RelationshipSaveState relationshipSaveState,
        WeatherSaveState weatherSaveState,
        float musicVolume,
        float sfxVolume,
        int currentMusicIndex

        )
    {

        this.farmSaveState = farmSaveState;
        this.inventorySaveState = inventorySaveState;
        this.timestamp = timestamp;
        //this.money = money;
        this.playerSaveState = playerSaveState;
        this.relationshipSaveState = relationshipSaveState;
        this.weatherSaveState = weatherSaveState;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.currentMusicIndex = currentMusicIndex;
    }
    */

    public GameSaveState(GameBlackboard blackboard,
                        FarmSaveState farmSaveState, 
                        InventorySaveState inventorySaveState, 
                        GameTimestamp timestamp, 
                        PlayerSaveState playerSaveState, 
                        WeatherSaveState weatherSaveState,
                        float musicVolume,
                        float sfxVolume,
                        int currentMusicIndex)
    {
        this.blackboard = blackboard;
        this.farmSaveState = farmSaveState;
        this.inventorySaveState = inventorySaveState;
        this.timestamp = timestamp;
        this.playerSaveState = playerSaveState;
        this.weatherSaveState = weatherSaveState ;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.currentMusicIndex = currentMusicIndex;

    }
}

