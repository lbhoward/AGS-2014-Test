using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	CharacterController charCont;
	GameObject mCamera;
	
	// Use this for initialization
	void Start () {
		//CharCont accessor (for using simple move)
		charCont = gameObject.GetComponent<CharacterController>();
		
		mCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
	
		RotatePlayerToMouse();
		
		WASDMovement();
    }
	
	// Rotates the player character to always face the mouse.
	// Also keeps camera positioned above player, top-down, with NO rotation applied
	void RotatePlayerToMouse()
	{
		//Find 3D position from mouse and rotate player to face
		Quaternion oldRotation = transform.rotation;
		
		RaycastHit hit;
    	if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
    		transform.LookAt(hit.point);
				
		transform.rotation = Quaternion.Lerp(oldRotation, transform.rotation, 0.075f);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		mCamera.transform.eulerAngles = new Vector3(90,0,0);
	}
	
	// Basic 8-Directional Movement which is NOT based on Player facing, intentionally.
	void WASDMovement()
	{
		//Move in basic WASD
		Vector3 movementVector = Vector3.zero;
		if (Input.GetKey("w"))
			movementVector.z += 5;
		if (Input.GetKey("s"))
			movementVector.z -= 5;
		if (Input.GetKey("a"))
			movementVector.x -= 5;
		if (Input.GetKey("d"))
			movementVector.x += 5;
		//Finally apply movement
		charCont.SimpleMove(movementVector);
	}
}
