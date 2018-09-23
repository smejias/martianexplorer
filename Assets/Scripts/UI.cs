using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text damage;   
    public GameObject player;
    private float _playerCurrentHealth;

    void Start ()
    {        
        
	}
	
	void Update ()
    {
        CurrentLife();
    }

    void CurrentLife()
    {
<<<<<<< HEAD
        _playerCurrentHealth = player.GetComponent<Character>().currentHealth;
        damage.text = "Life: " + _playerCurrentHealth;
=======
        if (player != null)
        {
            _playerCurrentHealth = player.GetComponent<Character>().currentHealth;
            damage.text = "Life: " + _playerCurrentHealth;
        }
>>>>>>> Testing-CCC
    }

    public void ShootingUI(bool state)
    {
        //Cursor.visible = state;
    }
}
