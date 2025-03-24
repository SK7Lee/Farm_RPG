using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    public static LandManager Instance { get; private set; }
    public static Tuple<List<LandSaveState>, List<CropSaveState>> farmData = null;
    List<Land> landPlots = new List<Land>();

    //Save states
    List<LandSaveState> landData = new List<LandSaveState>();
    List<CropSaveState> cropData = new List<CropSaveState>();
    private void Awake()
    {
        //If there is more than one instance of this class, destroy the new one
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the instance to this object
            Instance = this;
        }
    }

    void OnEnable()
    {
        RegisterLandPlots();
        StartCoroutine(LoadFarmData());

    }

    IEnumerator LoadFarmData() 
    { 
        yield return new WaitForEndOfFrame();
        if (farmData != null)
        {
            ImportLandData(farmData.Item1);
            ImportCropData(farmData.Item2);
        }
    }

    private void OnDestroy()
    {
        farmData = new Tuple<List<LandSaveState>, List<CropSaveState>>(landData, cropData);
    }

    #region Register&DeRegister
    void RegisterLandPlots()
    {
        foreach(Transform landTransform in transform)
        {
            Land land = landTransform.GetComponent<Land>();
            landPlots.Add(land);

            //Creare corresponding save state
            landData.Add(new LandSaveState());

            land.id = landPlots.Count - 1;

        }
    }

    public void RegisterCrop(int landID, SeedData seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        cropData.Add(new CropSaveState(landID, seedToGrow.name, cropState, growth, health));
    }

    public void DeregisterCrop(int landID)
    {
        cropData.RemoveAll(x => x.landID == landID);
    }
    #endregion
    #region State Changes
    public void OnLandStateChange(int id, Land.LandStatus landStatus, GameTimestamp lastWatered)
    {
        landData[id] = new LandSaveState(landStatus, lastWatered);
    }

    public void OnCropStateChange(int landId,  CropBehaviour.CropState cropState, int growth, int health)
    {
        int cropIndex = cropData.FindIndex(x => x.landID == landId);
        string seedToGrow = cropData[cropIndex].seedToGrow;
        cropData[cropIndex] = new CropSaveState(landId, seedToGrow, cropState, growth, health);
    }
    #endregion
    #region Loading Data
    public void ImportLandData(List<LandSaveState> landDatasetToLoad)
    {
        for (int i = 0; i < landDatasetToLoad.Count; i++) 
        { 
            LandSaveState landDataToLoad = landDatasetToLoad[i];
            landPlots[i].LoadLandData(landDataToLoad.landStatus, landDataToLoad.lastWatered);
        }
        landData = landDatasetToLoad;

    }

    public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
    {
        cropData = cropDatasetToLoad;
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }
}
