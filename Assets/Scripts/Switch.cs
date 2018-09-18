using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public GameObject ps, Door;
    public Material offMaterial, lightOn;
    private GameObject player;
    private Vector3 playerPosition;
    public bool isInteractive;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        playerPosition = player.transform.position;

        if (isInteractive == true)
        {
            Interaction();
        }
        
    }

    void Hit()
    {
        ps.GetComponent<Fire>().switchOn = false;        
        FindObjectOfType<AudioManager>().Play("Transformer Down");
        GetComponent<Renderer>().material = offMaterial;
    }
       
    void Interaction()
    {

        if (Vector3.Distance(transform.position, playerPosition) < 3f)
        {
            GetComponent<Renderer>().material = lightOn;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Door.GetComponent<Door>().openDoor = true;
            }
        }
        else
        {
            GetComponent<Renderer>().material = offMaterial;
        }
    }
}


