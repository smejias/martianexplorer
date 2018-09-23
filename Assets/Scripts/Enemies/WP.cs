using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class WP : MonoBehaviour {

	public Transform[] waypoints;

	void Start () {
		waypoints = new Transform[transform.childCount];

		for(int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = transform.GetChild(i);
		}
	}

	void Update () {
	}
=======
public class WP : MonoBehaviour
{

    public Transform[] waypoints;

    void Start()
    {
        waypoints = new Transform[transform.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
    }
>>>>>>> Testing-CCC
}