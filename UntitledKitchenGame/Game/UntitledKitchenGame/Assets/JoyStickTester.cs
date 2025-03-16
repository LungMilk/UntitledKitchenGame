using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : MonoBehaviour
{
    public string xAxis = "Horizontal";  // Right joystick horizontal axis
    public string yAxis = "Vertical";    // Right joystick vertical axis
    public List<string> input;

    void Update()
    {
        // Get the names of all connected joysticks
        string[] joystickNames = Input.GetJoystickNames();

        //Print out the names of all the connected joysticks with their indices
        //for (int i = 0; i < joystickNames.Length; i++)
        //{
        //    if (!string.IsNullOrEmpty(joystickNames[i]))  // Check if a joystick is connected
        //    {
        //        Debug.Log($"Joystick {i} is: {joystickNames[i]}");
        //    }
        //    else
        //    {
        //        Debug.Log($"Joystick {i} is not connected.");
        //    }
        //}

        // Get input values for the specified joystick axes (e.g., RHorizontal, RVertical)
        float horizontal = Input.GetAxis(xAxis);
        float vertical = Input.GetAxis(yAxis);

        // Check if joystick input is not 0 (i.e., the user is moving the joystick)
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // Display a message if the input is not zero (significant input)
            Debug.Log($"Joystick input: X = {horizontal}, Y = {vertical}");
        }
    }
}



