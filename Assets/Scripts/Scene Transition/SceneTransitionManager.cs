﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    
    //the blackboard key for location
    const string LOCATION_KEY = "Location";
    const string INDOORS = "CurrentlyIndoor";

    //the scenes the player can be in
    public enum Location
    {
        Farm, Town, PlayerHome, ChickenCoop, Forest, YodelRanch, Inn 
    }
    public Location currentLocation;

    //list of all indoor locations
    static readonly Location[] indoor = {Location.PlayerHome, Location.ChickenCoop, Location.YodelRanch, Location.Inn };
    // transform of the player
    Transform playerPoint;

    //check if the screen has finished fading out
    bool screenFadedOut;

    public UnityEvent onLocationLoad;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);            
        }
        else
        {

           Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnLocationLoad;

        //Find player transform
        playerPoint = FindAnyObjectByType<PlayerController>().transform;

    }

    public bool CurrentlyIndoor()
    {
        return indoor.Contains(currentLocation);
    }

    public void SwitchLocation(Location locationToSwitch)
    {
        UIManager.Instance.FadeOutScreen();
        screenFadedOut = false;
        StartCoroutine(ChangeScene(locationToSwitch));
        //SceneManager.LoadScene(locationToSwitch.ToString());
    }

    IEnumerator ChangeScene(Location locationToSwitch)
    {
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;
        while (!screenFadedOut)
        {
            yield return new WaitForSeconds(0.1f);
        }
        screenFadedOut = false;
        UIManager.Instance.ResetFadeDefaults();
        SceneManager.LoadScene(locationToSwitch.ToString());
        playerCharacter.enabled = true;

    }

    public void OnFadeOutComplete()
    {
        //Disable the fade out screen
        screenFadedOut = true;
    }

    //Called when a scene is loaded
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        Location oldLocation = currentLocation;
        Location newLocation =(Location) Enum.Parse(typeof(Location), scene.name);
        if (currentLocation == newLocation)
        {
            return;
        }
        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);

        if (playerPoint == null) return;

        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        playerPoint.position = startPoint.position;
        playerCharacter.enabled = true;

        //save the current location that we just switched to
        currentLocation = newLocation;

        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();
        blackboard.SetValue(LOCATION_KEY, currentLocation);
        blackboard.SetValue(INDOORS, CurrentlyIndoor());

        onLocationLoad?.Invoke();

        CutsceneManager.Instance.OnLocationLoad();
    }

}
