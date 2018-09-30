using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text damage;   
    public GameObject player;
    public Slider healthSlider;
    public Image bloodImage;
    private float _playerCurrentHealth;

    void Start ()
    {
    }
	
	void Update ()
    {
        CurrentLife();
    }

    private void CurrentLife()
    {
        if (player != null && player.GetComponent<Character>().IsAlive)
        {
            DamageUI(_playerCurrentHealth != player.GetComponent<Character>().currentHealth);
            _playerCurrentHealth = player.GetComponent<Character>().currentHealth;
            damage.text = "Life: " + _playerCurrentHealth;
            healthSlider.value = _playerCurrentHealth / 100;
        }
    }

    private void DamageUI(bool state)
    {
        if (state)
        {
            Color Opaque = Color.red;
            bloodImage.color = Color.Lerp(bloodImage.color, Opaque, 500 * Time.deltaTime);
            if (bloodImage.color.a >= 0.8)
            {
                state = false;
            }
        }
        if (!state)
        {
            Color Transparent = new Color(1, 1, 1, 0);
            bloodImage.color = Color.Lerp(bloodImage.color, Transparent, 20 * Time.deltaTime);
        }
    }    
}
