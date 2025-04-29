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

    ////Relationships
    //public List<NPCRelationshipState> relationships;
    //public List<AnimalRelationshipState> animals;

    ////Animals
    //public List<EggIncubationSaveState> eggsIncubating;

    public RelationshipSaveState relationshipSaveState;
    public GameBlackboard blackboard;

    public GameSaveState(

        FarmSaveState farmSaveState,

        InventorySaveState inventorySaveState,


        GameTimestamp timestamp,

        //int money, 
        PlayerSaveState playerSaveState,

        RelationshipSaveState relationshipSaveState

        )
    {

        this.farmSaveState = farmSaveState;

        this.inventorySaveState = inventorySaveState;


        this.timestamp = timestamp;

        //this.money = money;
        this.playerSaveState = playerSaveState;
        this.relationshipSaveState = relationshipSaveState;

    }

    public GameSaveState(GameBlackboard blackboard, FarmSaveState farmSaveState, InventorySaveState inventorySaveState, GameTimestamp timestamp, PlayerSaveState playerSaveState)
    {
        this.blackboard = blackboard;
        this.farmSaveState = farmSaveState;
        this.inventorySaveState = inventorySaveState;
        this.timestamp = timestamp;
        this.playerSaveState = playerSaveState;
    }
}

