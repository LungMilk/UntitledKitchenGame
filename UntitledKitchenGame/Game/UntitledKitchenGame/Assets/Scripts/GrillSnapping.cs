using System.Collections;
using UnityEngine;

public class Grill : MonoBehaviour
{
    public float detectionRadius = 5f;
    public Transform anchorPosition; // The position where the object should be moved to on the grill
    public float cookingTimeIncrease = 5f; // Additional time added to the object's cooking time by the grill
    public LayerMask cookableLayerMask;
    public AnimationCurve movementCurve; // The curve that defines the movement speed over time
    private bool anchored;

    public void Start()
    {
        anchorPosition = this.transform;
    }
    private void Update()
    {
        // Perform a check for any cookable objects within the detection radius
        if (!anchored) { 
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRadius, cookableLayerMask);

        foreach (Collider detected in detectedObjects)
        {
            // Check if the detected object is a cookable object
            CookableMeat cookable = detected.GetComponent<CookableMeat>();
            print(cookable.name);
            if (cookable != null && cookable.currentState == CookState.Uncooked)
            {
                anchored = true;
                // Disable collisions with the uncooked object
                cookable.SetCollisionsEnabled(false);

                // Start moving the object to the grill anchor position
                MoveObjectToGrillAnchor(detected.transform, anchorPosition.position);
                // Start cooking process
                cookable.StartCooking(cookingTimeIncrease);
            }
        }
    }
    }
    public void MoveObjectToGrillAnchor(Transform objectTransform, Vector3 targetPosition)
    {
        float moveTime = 5f; // Total time to move to the target position
        float elapsedTime = 0f;

        // Optionally, disable gravity while moving
        Rigidbody rb = objectTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Disable gravity during movement
        }

        while (elapsedTime < moveTime)
        {
            // Evaluate the curve at the current time fraction (elapsedTime / moveTime)
            float curveValue = movementCurve.Evaluate(elapsedTime / moveTime);

            // Lerp the position based on the curve value (smoothing)
            objectTransform.position = Vector3.Lerp(objectTransform.position, targetPosition, curveValue);

            elapsedTime += Time.deltaTime;
        }

        // Ensure the object reaches the target position
        objectTransform.position = targetPosition;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
