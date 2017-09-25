using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    float moveSpeed = 7f;
    public float smoothMoveTime = 0.1f;

    float smoothInputMagnitude;

    void Update()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;

        float inputMagnitude = inputDirection.magnitude;

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        transform.eulerAngles = Vector3.up * targetAngle;

        transform.Translate(transform.forward * moveSpeed * Time.deltaTime * inputMagnitude, Space.World);
    }
}
