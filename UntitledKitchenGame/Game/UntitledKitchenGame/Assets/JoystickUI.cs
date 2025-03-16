using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class JoystickUI : MonoBehaviour
{
    private Transform thisPos;
    public float maxMoveDis;

    private Vector2 inputVector;

    public string xAxis;
    public string yAxis;
    // Start is called before the first frame update
    void Start()
    {
        thisPos = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis(xAxis);
        float vertical = Input.GetAxis(yAxis);
        inputVector = new Vector2(horizontal, vertical);

        MoveJoystickKnob(inputVector);
    }
    private void MoveJoystickKnob(Vector2 input)
    {
        // Get the direction the knob should move (based on input, clamped to a unit circle)
        Vector2 direction = input.normalized;

        // Calculate the desired position for the knob based on the direction and maximum distance
        Vector2 targetPosition = direction * maxMoveDis;

        // Move the knob to the new position
        thisPos.localPosition = targetPosition;
    }
}
