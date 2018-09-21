using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnly : InteractableObject {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public override void Activate(Vector3 playerPosition)
    {
    }

    public void Hit()
    {
        if (ps != null)
        {
            ps.GetComponent<Fire>().switchOn = false;
        }

        FindObjectOfType<AudioManager>().Play("Transformer Down");
        GetComponent<Renderer>().material = offMaterial;
        GetComponent<Renderer>().material = lightOn;
    }
}
