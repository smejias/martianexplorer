﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrified : Obstacle {

	void Start () {
		
	}

	void Update () {
		
	}

   void OnTriggerStay (Collider collision)
    {
        if (collision.gameObject.tag == "Player")        
        {
            collision.SendMessageUpwards("TakeDamage", damage);
        }
    }     
}