using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
public Camera camera;
	public float interpVelocity;
	public float minDistance;
	public float followDistance;
	public GameObject target1;
    public GameObject target2;
	public Vector3 offset;
	Vector3 targetPos;
	// Use this for initialization
	void Start () {
		targetPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target1)
		{
			Vector3 posNoZ1 = transform.position;
			posNoZ1.z = target1.transform.position.z;
            Vector3 posNoZ2 = transform.position;
            posNoZ2.z = target2.transform.position.z;


			Vector3 targetDirection = ((target1.transform.position+target2.transform.position)/2 - (posNoZ1+posNoZ2)/2);

			interpVelocity = targetDirection.magnitude * 5f;

			targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 

			transform.position = Vector3.Lerp( transform.position, targetPos + offset, 0.25f);

		}
		var scrWheel = Input.GetAxis("Mouse ScrollWheel");
		if(scrWheel > 0f && camera.orthographicSize != 0){
			camera.orthographicSize += -Mathf.Abs(scrWheel*3);
		} else if(scrWheel < 0f && camera.orthographicSize != 10){
			camera.orthographicSize += Mathf.Abs(scrWheel*3);
		}
	}
}
