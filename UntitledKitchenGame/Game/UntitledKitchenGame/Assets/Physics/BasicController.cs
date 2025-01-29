using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour {
    public string horizontalAxis;
    public string verticalAxis;

    public float maxSpeed = 10;

    public float maxAccel = 10;

    [SerializeField, HideInInspector]
    Rigidbody _body;

    void OnValidate() {
        if (_body == null)
            TryGetComponent(out _body);
    }

    void FixedUpdate() {
        var input = new Vector3(
            Input.GetAxis(horizontalAxis),
            0,
            Input.GetAxis(verticalAxis)
        );

        var desiredVel = maxSpeed * input;

        var accel = (desiredVel - _body.velocity) / Time.deltaTime;

        accel = Vector3.ClampMagnitude(accel, maxAccel);
        
        _body.AddForce(accel, ForceMode.Acceleration);
    }
}
