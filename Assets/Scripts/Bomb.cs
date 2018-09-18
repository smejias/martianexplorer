using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    
    public Material lightOn;
    private GameObject player;
    private Vector3 playerPosition;
    public bool isInteractive;
    

    public float radius = 5.0F;
    public float power = 10.0F;

    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

    }
	
	// Update is called once per frame
	void Update () {

        playerPosition = player.transform.position;

        if (isInteractive == true)
        {
            Interaction();
        }

    }

    void Interaction()
    {

        if (Vector3.Distance(transform.position, playerPosition) < 3f)
        {
            GetComponent<Renderer>().material = lightOn;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 explosionPos = transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                        rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                }
            }
        }
        
    }
}
