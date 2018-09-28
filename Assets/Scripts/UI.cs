using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text damage;   
    public GameObject player;
    public Slider healthSlider;
    public Texture2D initialCursor;
    private float _playerCurrentHealth;

    void Start ()
    {
        StartCursor();
    }
	
	void Update ()
    {
        CurrentLife();
    }

    void CurrentLife()
    {
        if (player != null)
        {
            _playerCurrentHealth = player.GetComponent<Character>().currentHealth;
            damage.text = "Life: " + _playerCurrentHealth;
            healthSlider.value = _playerCurrentHealth / 100;
        }
    }

    public void StartCursor()
    {
        Cursor.SetCursor(initialCursor, Vector2.zero, CursorMode.Auto);       
    }
}
