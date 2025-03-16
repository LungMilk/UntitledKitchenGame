using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodChange : MonoBehaviour
{
    public GameObject uncook; // Reference to the uncook food prefab
    public GameObject Cooked;  // Reference to the cooked food prefab
    public Transform cookingLocationR;  // The location of the cooking area futher from the screen
    public Transform cookingLocationL;  // The location of the cooking area closer from the screen
    public float snapRadius = 5f;   // The radius within which the uncook food prefab will snap
    public float delayBeforeSwitch = 10f; // Delay before switching to the cooked food prefab

    private bool hasSnapped = false;
    private float switchTimer = 0f; // Timer to track the delay before switching
    private Transform currentCookingLocation; //The current location of the cooking station

    //public float movementSpeed = 5f; // Speed of WASD movement

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Before prefab is within the snap radius of either cooking station
        if (!hasSnapped)
        {
            CheckForSnap();
        }

        // Allow movement only if the prefab has not snapped yet
        //if (!hasSnapped)
        //{
        //    HandleMovement();
        //}

        // Update the timer if the prefab has snapped
        if (hasSnapped)
        {
            UpdateSwitchTimer();
        }
    }

    void CheckForSnap()
    {
        if (cookingLocationL == null || cookingLocationR ==null)
        {
            return;
        }
        // Calculate distances to both cooking stations
        float distanceToR = Vector3.Distance(transform.position, cookingLocationR.position);
        float distanceToL = Vector3.Distance(transform.position, cookingLocationL.position);

        // Check if the Before prefab is within the snap radius of the right station
        if (distanceToR <= snapRadius)
        {
            currentCookingLocation = cookingLocationR;
            SnapToCenter();
        }
        // Check if the Before prefab is within the snap radius of the left station
        else if (distanceToL <= snapRadius)
        {
            currentCookingLocation = cookingLocationL;
            SnapToCenter();
        }
    }


    //void HandleMovement()
    //{
    //    // Get input from WASD keys
    //    float moveX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime; // A and D keys
    //    float moveZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;   // W and S keys

    //    // Move the object
    //    transform.Translate(new Vector3(moveX, 0, moveZ));
    //}

    void SnapToCenter()
    {
        // Snap the uncook food prefab to the center of the target location
        transform.position = currentCookingLocation.position;

        // Disable the collider on the uncook food prefab
        Collider uncookCollider = GetComponent<Collider>();
        if (uncookCollider != null)
        {
            uncookCollider.enabled = false;
        }

        // Set the flag to indicate that the prefab has snapped
        hasSnapped = true;

        // Start the coroutine to switch to the cooked food prefab after a delay
        StartCoroutine(SwitchToCooked());
    }

    private System.Collections.IEnumerator SwitchToCooked()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeSwitch);

        // Instantiate the cooked food prefab at the same position and rotation
        GameObject afterInstance = Instantiate(Cooked, transform.position, transform.rotation);

        // Enable the collider on the cooked food prefab
        Collider afterCollider = afterInstance.GetComponent<Collider>();
        if (afterCollider != null)
        {
            afterCollider.enabled = true;
        }

        // Destroy the current instance of the uncook food prefab (GameObject)
        Destroy(gameObject);
    }

    void UpdateSwitchTimer()
    {
        // Increment the timer by the time passed since the last frame
        switchTimer += Time.deltaTime;

        // Calculate the remaining time
        float remainingTime = delayBeforeSwitch - switchTimer;

        // Log the remaining time to the console
        Debug.Log("Time remaining before switch: " + remainingTime.ToString("F2") + " seconds");

        // Optional: Stop logging if the time is up
        if (remainingTime <= 0)
        {
            Debug.Log("Switching to cooked food prefab!");
        }
    }


    // Draw Gizmos in the Scene view
    private void OnDrawGizmosSelected()
    {
       // Draw wireframe spheres for both cooking stations
        if (cookingLocationR != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(cookingLocationR.position, snapRadius);
            }

        if (cookingLocationL != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(cookingLocationL.position, snapRadius);
        }

    }
}
