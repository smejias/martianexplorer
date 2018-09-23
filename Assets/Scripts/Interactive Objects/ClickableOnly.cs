using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableOnly : InteractiveObject {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public override void Activate(Vector3 playerPosition)
    {
        if (Vector3.Distance(transform.position, playerPosition) < 3f)
        {
            base.Activate(playerPosition);
        }
        else
        {
            GetComponent<Renderer>().material = offMaterial;
        }
    }
}
