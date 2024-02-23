using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	[Header("References")]
	// [SerializeField] WallRun wallRun;

	private Transform cam;
	[SerializeField] private Transform orientation;
	//[SerializeField] GameObject playerModel;
	// [SerializeField] private Recoil recoil;

	public float mouseSensitivity = 100.0f;

	public Transform playerBody;
	public float xRotation = 0f;
	public float yRotation = 0f;

	private void Awake()
	{
		GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<MoveCamera>().SetCameraPosition();
		GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventory>().SetInventory();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
			
		cam = GameObject.FindGameObjectWithTag("CameraHolder").transform;
	}

	private void Update()
	{
		HandleCameraRotation();

		// yRotation = Mathf.Clamp(yRotation, )	

		cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, transform.rotation.z);
		orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
		//playerModel.transform.rotation = Quaternion.Euler(0, yRotation, 0);

		// if (trauma > 0.0f)
		// CameraShake();
		// Debug.Log(transform.localRotation.y * Mathf.Rad2Deg);
		// Debug.Log(xRotation + ", " + yRotation);
	}

	private void HandleCameraRotation()
	{
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		yRotation += mouseX * mouseSensitivity;
		xRotation -= mouseY * mouseSensitivity;

		xRotation = Mathf.Clamp(xRotation, -90, 90);
	}
}
