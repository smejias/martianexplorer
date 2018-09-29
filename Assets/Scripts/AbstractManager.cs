using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AbstractManager : MonoBehaviour
{
    public GameObject console;
    public GameObject pauseMenu;
    private Manager _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<Manager>();
        StartMenus();
    }

    void Update()
    {
    }

    public void LoadIntro()
    {
        _gameManager.LoadIntro();
    }

    public void ExitGame()
    {
        _gameManager.ExitGame();
    }
    
    private void StartMenus()
    {
        if (SceneManager.GetActiveScene().name == "Testing_CCC")
        {
            _gameManager.StartConsole(console);
            _gameManager.StartPauseMenu(pauseMenu);
        }
    }

    public void StartGame()
    {
        _gameManager.StartGame();
    }

    public void Continue()
    {
        _gameManager.Continue();
    }

    public void RestartGame()
    {
        _gameManager.RestartGame();
    }
}
