using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

	private int _currentWp;
	public WP waypoints;
	protected float speed = 5;
	protected bool canMove;
	public GameObject target;
	int direction;
	public Rigidbody rb;
	protected bool playerSpoted = false;

	private bool reverse;

	void Start () {
		canMove = true;
		rb = GetComponent<Rigidbody>();
		direction = 1;
	}

	void Update () {
		FindTarget();
		Move();
	}

	private void FindTarget()
	{
		if (target == null)
		{
			target = GameObject.FindGameObjectWithTag("Player") as GameObject;
		}
	}  

	public void MoveToWaypoints()
	{
		Vector3 distance = waypoints.waypoints[_currentWp].position - transform.position;
		if(distance.magnitude > speed * Time.deltaTime)
		{
			transform.position += distance.normalized * speed * Time.deltaTime;
			transform.right = Vector3.Lerp(transform.right, distance.normalized, 0.5f);
		}
		else
		{
			transform.position = waypoints.waypoints[_currentWp].position;
			_currentWp += direction;
			if (_currentWp >= waypoints.waypoints.Length || _currentWp < 0)
			{
				direction *= -1;
				_currentWp += direction;
			}
		}
	}

	public void Move()
	{
		if (canMove)
		{
			if (!playerSpoted) {
				MoveToWaypoints();		
			} else 
			{
				GoTo(target.transform);
			}
		}
	}	

	public void GoTo(Transform targetToFollow)
	{
		Vector3 dir;
		//dir = (targetToFollow.position - transform.position).normalized;

		//Vector3 velocity = dir * speed;
		//rb.velocity = new Vector3(velocity.x, velocity.y, velocity.z);
		//transform.position += velocity * Time.deltaTime;
	}
}
