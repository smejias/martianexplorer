using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrified : MonoBehaviour {

    public float damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
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
