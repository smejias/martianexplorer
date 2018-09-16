using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject console;
    private Console _consoleScript;
    public Character character;
    public Utils utils;
    private bool _paused;

    void Start () {
        _consoleScript = console.GetComponent<Console>();
        RegisterAllCommands();
        _paused = false;
    }
	
	void Update () {
        OpenConsole();
	}

    private void OpenConsole()
    {
        if (Input.GetKeyDown(utils.openConsole))
        {
            console.SetActive(!console.activeSelf);
            _consoleScript.Initialize();
            Pause(!_paused);
        }
    }

    public void RegisterAllCommands()
    {
        Console.instance.Registercomand("/help", _consoleScript.Help, utils.help);
        Console.instance.Registercomand("/godmode", character.GodMode, utils.godMode);
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(utils.pause) && !_paused)
        {
            Pause(!_paused);
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
        _paused = pause;
    }

    public bool Paused
    {
        get
        {
            return _paused;
        }

        set
        {
            _paused = value;
        }
    }
}
