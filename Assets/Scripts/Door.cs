using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField]
    public bool openDoor;

	void Start () {

        openDoor = false;


    }
	
	void Update () {

        if (openDoor == true)
        {
            gameObject.SetActive(false);
        }
		
	}
}
