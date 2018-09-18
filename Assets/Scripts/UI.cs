using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text damage;   
    public GameObject player;
    private float playerCurrentHealth;
    private float damageTaken;

    // Use this for initialization
    void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {

        CurrentLife();
    }

    void CurrentLife()
    {
        playerCurrentHealth = player.GetComponent<Character>().currentHealth;
        damage.text = "Life: " + playerCurrentHealth;
    }
}
