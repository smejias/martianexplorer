using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private Console _console;
    public Utils utils;
    public static Manager instance;
    private bool _paused = false;
    private bool _winCondition = false;


    public void Awake()
    {
        Singleton();
    }

    void Start () {

        DontDestroyOnLoad(gameObject);
    }
	
	void Update () {
        OpenConsole();
        PauseGame();
        Lose();
        Win();
    }

    private void Win()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemies");
        _winCondition = enemies.Length == 0;

        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Testing_CCC")
        {
            if (_winCondition)
            {
                WinScene();
                _winCondition = false;
            }
        }
    }   

    public void StartConsole(GameObject console)
    {
        if (_console == null)
        {
            _console = console.GetComponent<Console>();
            RegisterAllCommands();
        }
    }

    private void Lose()
    {
        if (Player() != null && !Player().IsAlive)
        {
            LoseScene();
        }
    }

    private void LoseScene()
    {
        SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
    }

    private void WinScene()
    {
        SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
    }

    public Character Player()
    {
        Character actualPlayer = (Character)FindObjectOfType(typeof(Character));
        return actualPlayer;
    }

    private void OpenConsole()
    {
        if (Input.GetKeyDown(utils.openConsole))
        {
            _console.gameObject.SetActive(!_console.gameObject.activeSelf);
            _console.Initialize();
            Pause(!_paused);
        }
    }

    public void Singleton()
    {
        var introScenes = GameObject.FindGameObjectsWithTag("GameManager");
        if (introScenes.Length > 1)
        {
            foreach (var scene in introScenes)
            {
                if (scene.GetInstanceID() != gameObject.GetInstanceID())
                {
                    Destroy(scene);
                }
            }
        }
    }

    public void RegisterAllCommands()
    {
        Console.instance.Registercomand("/help", _console.Help, utils.help);
        Console.instance.Registercomand("/godmode", Player().GodMode, utils.godMode);
        Console.instance.Registercomand("/fastermovement", Player().FasterMovement, utils.fasterMovement);
        Console.instance.Registercomand("/undetectable", Player().Undetect, utils.undetect);
        Console.instance.Registercomand("/win", ConsoleWin, utils.win);
        Console.instance.Registercomand("/lose", ConsoleLose, utils.lose);
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(utils.pause) && !_paused)
        {
            Pause(!_paused);
        }
        else if (Input.GetKeyDown(utils.pause) && _paused)
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

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ConsoleWin(bool state)
    {
            _console.gameObject.SetActive(!_console.gameObject.activeSelf);
            _console.Initialize();
            Pause(!_paused);
            WinScene();
    }

    public void ConsoleLose(bool state)
    {
            _console.gameObject.SetActive(!_console.gameObject.activeSelf);
            _console.Initialize();
            Pause(!_paused);
            LoseScene();
    }

    public void StartGame()
    {

        SceneManager.LoadScene("Testing_CCC", LoadSceneMode.Single);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("CreditsScene", LoadSceneMode.Single);
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene("IntroScene", LoadSceneMode.Single);
    }

}
