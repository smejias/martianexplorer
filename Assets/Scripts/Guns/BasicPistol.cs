using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPistol : Gun {

	void Start () {
      fireRate = 0.5f;
      weaponRange = 50;
      hitForce = 100;
      manager = GameObject.Find("GameManager").GetComponent<Manager>();
      _audioSource = GetComponent<AudioSource>();
      FindPlayer();
      SetGunDamage(25);
    }
	
	void Update () {
        laserLine = GetComponent<LineRenderer>();
    }

    public override void Shoot()
    {
        base.Shoot();
    }
}
