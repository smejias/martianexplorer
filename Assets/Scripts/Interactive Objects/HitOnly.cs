using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnly : InteractiveObject {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public override void Activate(Vector3 playerPosition)
    {
    }

    public override void Hit()
    {
        base.Hit();
    }
}
