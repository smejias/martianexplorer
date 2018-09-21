using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        PauseGame();
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

    public void WinLevel(int level)
    {
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("Level2", LoadSceneMode.Single);
                StartLevel();
                break;
            case 2:
                SceneManager.LoadScene("Level3", LoadSceneMode.Single);
                StartLevel();
                break;
            case 3:
                SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
                break;
            default:
                break;
        }
    }

    public int GetLevelNumber()
    {
        Scene scene = SceneManager.GetActiveScene();
        switch (scene.name)
        {
            case "IntroScene":
                return 0;
            case "Level1":
                return 1;
            case "Level2":
                return 2;
            case "Level3":
                return 3;
            case "Tutorial":
                return 99;
            default:
                return 4;
        }
    }

    public void StartLevel()
    {
        /*int numberLevel = GetLevelNumber();
        if (numberLevel != 0 && numberLevel != 4)
        {

        }
        winEnabled = true;
        alreadyRestarting = false*/

        // TO DO - Scene management and win condition
    }
}
