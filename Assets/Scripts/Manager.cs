using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
    
    public Utils utils;
    public static Manager instance;
    public Texture2D initialCursor;
    public Texture2D shootCursor;
    private GameObject _lifeBar;
    private bool _paused = false;
    private bool _winCondition = false;
    private Console _console;
    private GameObject _pauseMenu;
    private bool _canLose = true;
    private bool _winGame = false;

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

    public bool WinCondition
    {
        get
        {
            return _winCondition;
        }

        set
        {
            _winCondition = value;
        }
    }

    public bool WinGame
    {
        get
        {
            return _winGame;
        }

        set
        {
            _winGame = value;
        }
    }

    public void Awake()
    {
        Singleton();
    }

    void Start ()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	void Update () {
        OpenConsole();
        PauseGame();
        Lose();
        Win();
        CurrentCursor();
    }

    private void Win()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemies");
        WinCondition = enemies.Length == 0;

        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Testing_CCC")
        {
            if (WinCondition)
            {
                WinGame = true;
                WinCondition = false;
                WinScene();               
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

    public void StartPauseMenu(GameObject pauseMenu)
    {
        if (_pauseMenu == null)
        {
            _pauseMenu = pauseMenu;
            _pauseMenu.SetActive(false);
        }
    }

    private void Lose()
    {
        if (Player() != null && !Player().IsAlive && _canLose)
        {
            _canLose = false;
            Invoke("LoseScene", 4);
        }
    }

    private void LoseScene()
    {
        SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
    }

    private void WinScene()
    {
        SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
        WinGame = false;
    }

    public Character Player()
    {
        Character actualPlayer = (Character)FindObjectOfType(typeof(Character));
        return actualPlayer;
    }

    private void OpenConsole()
    {
        if (Input.GetKeyDown(utils.openConsole) && !_pauseMenu.activeInHierarchy)
        {
            OpenCloseConsole(!_console.gameObject.activeSelf);
            Pause(!_paused);
            _console.Initialize();
        }
    }

    private void OpenCloseConsole(bool state)
    {
        _console.gameObject.SetActive(state);
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
        if (Input.GetKeyDown(utils.pause))
        {
            PauseMenu(!_paused);
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
        
        TurnOnOffLifeBar();
        Time.timeScale = pauseScale;
        _paused = pause;
    }

    public void PauseMenu(bool state)
    {
        OpenCloseConsole(false);
        _pauseMenu.gameObject.SetActive(state);
    }

    public void Continue()
    {
        Pause(false);
        PauseMenu(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ConsoleWin(bool state)
    {
        OpenCloseConsole(false);
        Pause(!_paused);
        WinScene();
    }

    public void ConsoleLose(bool state)
    {
        OpenCloseConsole(false);
        Pause(!_paused);
        LoseScene();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Testing_CCC", LoadSceneMode.Single);
        _canLose = true;
    }

    public void RestartGame()
    {
        OpenCloseConsole(false);
        Pause(false);
        StartGame();
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("CreditsScene", LoadSceneMode.Single);
    }

    public void LoadIntro()
    {
        Pause(false);
        SceneManager.LoadScene("IntroScene", LoadSceneMode.Single);
    }

    public void TurnOnOffLifeBar()
    {
        if (_lifeBar == null)
        {
            _lifeBar = GameObject.Find("HealthBarBacking");
        }
        if (_lifeBar != null)
        {
            _lifeBar.gameObject.SetActive(!_lifeBar.gameObject.activeSelf);
        }
    }

    public void CurrentCursor()
    {
        if (!WinCondition && Player().ShootingOn && Player().IsAlive)
        {
            ShootCursor();
        }
        else
        {
            StartCursor();
        }
    }

    private void StartCursor()
    {
        if (initialCursor != null)
        {
            Cursor.SetCursor(initialCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private void ShootCursor()
    {
        if (initialCursor != null)
        {
            Cursor.SetCursor(shootCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
