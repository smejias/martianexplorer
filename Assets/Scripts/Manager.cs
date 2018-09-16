using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject console;
    private Console consoleScript;
    public Character character;
    public Utils utils;
    private bool paused;

    void Start () {
        consoleScript = console.GetComponent<Console>();
        RegisterAllCommands();
        paused = false;
    }
	
	void Update () {
        OpenConsole();
	}

    private void OpenConsole()
    {
        if (Input.GetKeyDown(utils.openConsole))
        {
            console.SetActive(!console.activeSelf);
            consoleScript.Initialize();
            Pause(!paused);
        }
    }

    public void RegisterAllCommands()
    {
        Console.instance.Registercomand("/help", consoleScript.Help, utils.help);
        Console.instance.Registercomand("/godmode", character.GodMode, utils.godMode);
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(utils.pause) && !paused)
        {
            Pause(!paused);
        }
    }
    
    public void Pause(bool pause)
    {
        int pauseScale;

        if (pause)
        {
            pauseScale = 0;
        }
        else
        {
            pauseScale = 1;
        }

        Time.timeScale = pauseScale;
        paused = pause;
    }

    public bool Paused
    {
        get
        {
            return paused;
        }

        set
        {
            paused = value;
        }
    }
}
