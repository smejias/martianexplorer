using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AbstractManager : MonoBehaviour
{
    private Manager _gameManager;
    public GameObject console;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<Manager>();
        StartConsole();
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
    
    private void StartConsole()
    {
        if (SceneManager.GetActiveScene().name == "Testing_CCC")
        {
            _gameManager.StartConsole(console);
        }
    }
}
