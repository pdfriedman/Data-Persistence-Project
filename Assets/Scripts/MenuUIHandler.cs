using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    
    public string playerName; // add new variable from start scene

    // Awake method added to communicate with start scene
    public static MenuUIHandler Instance;

    
    private void Awake()
    {
        // Singelton:  Remove duplicates of gameobject
        if (Instance != null)
        {
        Destroy(gameObject);
        return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public void NewStart()
    {
        SceneManager.LoadScene(1);
    }

    public void ReadStringInput(string s)
    {
        playerName = s;
        Debug.Log(playerName);

    }
}
