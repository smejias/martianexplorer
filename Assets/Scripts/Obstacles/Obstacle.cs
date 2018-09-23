using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    public float damage;

	void Start () {
		
	}
	
	void Update () {
		
	}
<<<<<<< HEAD
=======

    public void DoDamage(GameObject other, float damage)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemie")
        {
            other.SendMessageUpwards("TakeDamage", damage);
        }
    }
>>>>>>> Testing-CCC
}
