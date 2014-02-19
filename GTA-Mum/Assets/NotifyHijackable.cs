// Coded by Lawrence Howard - Advanced Game Studies 2013/2014
// 10183346@lincoln.ac.uk

using UnityEngine;
using System.Collections;

public class NotifyHijackable : MonoBehaviour {

	bool HijackAvailable = false;
	bool HijackInProgress = false;
	bool PlayerDriving = false;

	// Timer controls
	float HijackStarted = 0.0f;
	float HijackTimeElapsed = 0.0f;
	float HijackTimeRequired = 3.0f;

	// Driving controls
	GameObject Vehicle;
	float speed = 0;
	float MAX_SPEED = 20;
	float MIN_SPEED = -12.5f;

	// Use this for initialization
	void Start () {
		Vehicle = gameObject.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if (PlayerDriving)
		{
			if (Input.GetKeyDown(KeyCode.E))
				ExitCar();

			Movement();
			return;
		}

		if (HijackAvailable)
		{
			if (!HijackInProgress && Input.GetKey(KeyCode.E))
			{
				HijackInProgress = true;


				HijackStarted = Time.time;
			}
			else if (Input.GetKey(KeyCode.E))
			{
				HijackTimeElapsed = Time.time - HijackStarted;

				if (HijackTimeElapsed >= HijackTimeRequired)
				{
					EnterCar();
				}
			}
			else
			{
				HijackInProgress = false;
			}
		}
	
	}

	// Called whenever Player is driving this vehicle
	void Movement()
	{

		//Steering
		if (Input.GetKey(KeyCode.A))
			Vehicle.transform.RotateAround(transform.position, Vector3.up, -2.5f);
		if (Input.GetKey(KeyCode.D))
			Vehicle.transform.RotateAround(transform.position, Vector3.up, 2.5f);

		//Forward and Reverse
		if (Input.GetKey(KeyCode.W))
			speed += 0.25f;
		if (Input.GetKey(KeyCode.S))
			speed -= 0.1f;

		speed = Mathf.Clamp(speed, MIN_SPEED,  MAX_SPEED);

		Vehicle.rigidbody.velocity = Vehicle.transform.forward * speed;

	}

	// Called whenever the Player completes a Hijack
	void EnterCar()
	{
		// Locate the Player & Players Script for access
		GameObject PlayerObj = GameObject.FindGameObjectWithTag("Player"); // Player Object
		Player PlayerScript = PlayerObj.GetComponent<Player>();		// Player Script

		PlayerObj.renderer.enabled = false; // Hide the player
		foreach (Renderer r in PlayerObj.GetComponentsInChildren<Renderer>()) // And any weapons etc
		{
			r.enabled = false;
		}

		PlayerScript.transform.position = transform.position; // Move player to this vehicle
		PlayerObj.GetComponent<CharacterController>().enabled = false; // And ensure player collisions are off

		PlayerScript.isDriving = true; // Stop WASD movement on PlayerObj
		PlayerScript.FollowWhileDriving = Vehicle.transform; // And follow this car with the camera

		PlayerDriving = true; // And finally notify this vehicle we can drive!
		HijackInProgress = false; // And reset for new Hijack.
		HijackAvailable = false;

	}

	// Called whenever the Player exits the vehicle
	void ExitCar()
	{
		// Locate the Player & Players Script for access
		GameObject PlayerObj = GameObject.FindGameObjectWithTag("Player"); // Player Object
		Player PlayerScript = PlayerObj.GetComponent<Player>();		// Player Script
		
		PlayerObj.renderer.enabled = true; // Show the player
		foreach (Renderer r in PlayerObj.GetComponentsInChildren<Renderer>()) // And any weapons etc
		{
			r.enabled = true;
		}
		
		PlayerScript.transform.position = (transform.position+(transform.right*5))+(transform.up); // Move player to this vehicles side
		PlayerObj.GetComponent<CharacterController>().enabled = true; // And ensure player collisions are on
		
		PlayerScript.isDriving = false; // Enable WASD movement on PlayerObj
	}

	void OnGUI()
	{
		if (HijackInProgress)
		{
			GUI.Box(new Rect((Screen.width/2)-200, Screen.height-200, 400, 60), "Hijacking...");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		// If it is the player that has entered my Hijack Zone
		if (other.gameObject.transform.name == "PH_Character")
		{
			// Allow this car to be hijacked
			HijackAvailable = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		// If it is the player that has exited my Hijack Zone
		if (other.gameObject.transform.name == "PH_Character")
		{
			// No longer allow  me to be hijacked
			HijackAvailable = false;
		}
	}
}
