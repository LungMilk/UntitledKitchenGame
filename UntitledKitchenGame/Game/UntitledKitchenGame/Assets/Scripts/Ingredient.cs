using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient: MonoBehaviour
{
    [SerializeField] Material UNcookedMaterial;  // Reference to the cooked food material
    [SerializeField] Material cookedMaterial;  // Reference to the cooked food material
    //public Transform cookingLocationR;  // The location of the cooking area further from the screen
    //public Vector3 cookingLocationL;  // The location of the cooking area closer to the screen
    //public Vector3 cookingLocationC;  // The third location of the cooking area 
    //public float snapRadius = 5f;   // The radius within which the uncooked food prefab will snap
    [SerializeField] float delayBeforeSwitch = 10f; // Delay before switching to the cooked food texture
    //public float movementSpeed = 5f; // Speed of movement for the food

    //private bool hasSnapped = false;
    //private float switchTimer = 0f; // Timer to track the delay before switching
    //private Vector3 currentCookingLocation; // The current location of the cooking station
    private Renderer[] foodRenderer; // Reference to the Renderer component of the food

    bool iscooked;

    void Awake()
    {
        try
        {
            foodRenderer=GetComponentsInChildren<Renderer>();
            changeMatierial(UNcookedMaterial);
        }
        catch(SystemException e)
        {
            Debug.LogError("can't find render\n" + e.Message);
        }
        
    }
    void changeMatierial(Material material)
    {
        foreach(var render in foodRenderer)
        {
            render.material = material;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    // Allow movement only if the food has not snapped yet
    //    if (!hasSnapped)
    //    {
    //        //HandleMovement();
    //        CheckForSnap();
    //    }

    //    // Update the timer if the food has snapped
    //    if (hasSnapped)
    //    {
    //        UpdateSwitchTimer();
    //    }
    //}

    //void HandleMovement()
    //{
    //    // Get input from WASD keys or arrow keys
    //    float moveX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime; // A/D or Left/Right Arrow
    //    float moveZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;   // W/S or Up/Down Arrow

    //    // Move the object
    //    transform.Translate(new Vector3(moveX, 0, moveZ));
    //}

    //void CheckForSnap()
    //{
    //    // Calculate distances to all cooking stations
    //    float distanceToR = Vector3.Distance(transform.position, cookingLocationR);
    //    float distanceToL = Vector3.Distance(transform.position, cookingLocationL);
    //    float distanceToC = Vector3.Distance(transform.position, cookingLocationC);

    //    // Debug logs to check distances
    //    Debug.Log("Distance to R: " + distanceToR);
    //    Debug.Log("Distance to L: " + distanceToL);
    //    Debug.Log("Distance to C: " + distanceToC);

    //    // Check if the food is within the snap radius of any station
    //    if (distanceToR <= snapRadius)
    //    {
    //        Debug.Log("Snapping to cookingLocationR");
    //        currentCookingLocation = cookingLocationR;
    //        SnapToCenter();
    //    }
    //    else if (distanceToL <= snapRadius)
    //    {
    //        Debug.Log("Snapping to cookingLocationL");
    //        currentCookingLocation = cookingLocationL;
    //        SnapToCenter();
    //    }
    //    else if (distanceToC <= snapRadius)
    //    {
    //        Debug.Log("Snapping to cookingLocationC");
    //        currentCookingLocation = cookingLocationC;
    //        SnapToCenter();
    //    }
    //}

    //void SnapToCenter()
    //{
    //    if (currentCookingLocation == null)
    //    {
    //        Debug.LogError("currentCookingLocation is not assigned!");
    //        return;
    //    }

    //    // Snap the food to the center of the target location
    //    transform.position = currentCookingLocation;
    //    Debug.Log("Snapped to: " + currentCookingLocation);

    //    // Disable the collider on the current prefab only
    //    Collider foodCollider = GetComponent<Collider>();
    //    if (foodCollider != null)
    //    {
    //        foodCollider.enabled = false;
    //        Debug.Log("Collider disabled on current prefab.");
    //    }
    //    else
    //    {
    //        Debug.LogError("No collider found on the current prefab!");
    //    }

    //    // Set the flag to indicate that the food has snapped
    //    hasSnapped = true;

    //    // Start the coroutine to switch to the cooked texture after a delay
    //    StartCoroutine(SwitchToCooked());
    //}

    public void cook()
    {
        if (iscooked) return;
        StartCoroutine(SwitchToCooked());
    }

    private IEnumerator SwitchToCooked()
    {
       
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeSwitch);

        // Change the material to the cooked material
        changeMatierial(cookedMaterial);

        //if (cookedMaterial != null && foodRenderer != null)
        //{

        //    Debug.Log("Material changed to cooked material.");
        //}
        //else
        //{
        //    Debug.LogError("Cooked material or Renderer is not assigned!");
        //}

        // Enable the collider on the current prefab
        //Collider foodCollider = GetComponent<Collider>();
        //if (foodCollider != null)
        //{
        //    foodCollider.enabled = true;
        //    Debug.Log("Collider enabled on current prefab.");
        //}
    }

    //void UpdateSwitchTimer()
    //{
    //    // Increment the timer by the time passed since the last frame
    //    switchTimer += Time.deltaTime;

    //    // Calculate the remaining time
    //    float remainingTime = delayBeforeSwitch - switchTimer;

    //    // Log the remaining time to the console
    //    Debug.Log("Time remaining before texture change: " + remainingTime.ToString("F2") + " seconds");

    //    // Optional: Stop logging if the time is up
    //    if (remainingTime <= 0)
    //    {
    //        Debug.Log("Changing to cooked texture!");
    //    }
    //}

    //// Draw Gizmos in the Scene view
    //private void OnDrawGizmosSelected()
    //{
    //    // Draw wireframe spheres for all cooking stations
    //    if (cookingLocationR != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(cookingLocationR, snapRadius);
    //    }

    //    if (cookingLocationL != null)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireSphere(cookingLocationL, snapRadius);
    //    }

    //    if (cookingLocationC != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(cookingLocationC, snapRadius);
    //    }
    //}

}
