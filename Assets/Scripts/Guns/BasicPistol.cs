using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPistol : Gun {

	void Start () {
      GunDamage = 1;
      fireRate = 0.5f;
      weaponRange = 50;
      hitForce = 100;
      manager = GameObject.Find("GameManager").GetComponent<Manager>();
      _initialGunDamage = GunDamage;
    }
	
	void Update () {
        laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
    }

    public override void Shoot()
    {
        base.Shoot();
    }
}
