using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float moveSpeed = 7f;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8f;

    float smoothInputMagnitude;
    float smoothMoveVelocity;
    float angle;
    Vector3 velocity;

    Rigidbody rb;
    bool disabled;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disabled;
    }

    void Update()
    {
        Vector3 inputDirection = Vector3.zero;

        if (!disabled)
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        
        float inputMagnitude = inputDirection.magnitude;

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

        velocity = transform.forward * moveSpeed * smoothInputMagnitude;

        //transform.eulerAngles = Vector3.up * angle;

        //transform.Translate(transform.forward * moveSpeed * Time.deltaTime * smoothInputMagnitude, Space.World);
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));

        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void Disabled()
    {
        disabled = true;
    }

    private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Disabled;
    }
}
