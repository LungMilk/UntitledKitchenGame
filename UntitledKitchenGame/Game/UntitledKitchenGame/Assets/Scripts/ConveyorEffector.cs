using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float pushForce = 5f;  // Force applied to move the object
    public Vector3 pushDirection = Vector3.forward;  // Direction in which to push the objects (default: forward)

    private void OnTriggerStay(Collider other)
    {
        print($"{other.name} is on the conveyor");
        // Check if the object has a "Item" tag (you can set your own tag for items)
        //if (other.CompareTag("Food"))
        //{
            // Apply a force to push the object in the desired direction
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Apply force to the Rigidbody in the specified direction
                rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
            }
        //}
    }
    private void OnDrawGizmos()
    {
        // Set gizmo color to yellow (or any other color you prefer)
        Gizmos.color = Color.yellow;

        // Draw an arrow representing the direction of movement
        // The arrow starts at the conveyor belt's position and points in the pushDirection
        Gizmos.DrawRay(transform.position, pushDirection.normalized * 2f);  // Adjust length as needed

        // Optionally, you can add some text to indicate direction more clearly:
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + pushDirection.normalized * 2f, 0.1f);  // Draw a small sphere at the arrow's tip
    }
}
