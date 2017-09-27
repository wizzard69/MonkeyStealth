using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform player;
	public Vector3 offset;
	public float smoothSpeed;
    private Vector3 desiredPosition;

    private void LateUpdate()
    {
		//Vector3 newPosition = player.position;

		//newPosition.y = transform.position.y;

		//transform.position = newPosition;

		desiredPosition = player.position + offset;

		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

		transform.position = smoothedPosition;

		transform.LookAt(player);
    }
}
