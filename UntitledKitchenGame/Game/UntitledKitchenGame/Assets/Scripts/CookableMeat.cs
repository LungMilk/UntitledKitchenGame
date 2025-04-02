using UnityEngine;

public enum CookState { Uncooked, Cooking, Cooked }

public class CookableMeat : MonoBehaviour
{
    public TimerUI timer;
    public CookState currentState = CookState.Uncooked;
    public float cookingTime = 0f;
    public float maxCookingTime = 10f; // Total time to fully cook
    private float cookingTimer = 0f;

    public Material uncookedMaterial; // Material for uncooked state
    public Material cookedMaterial; // Material for cooked state
    private Renderer meatRenderer; // Renderer of the meat object
    public float cookingProgress = 0f; // Progress of cooking (0 to 1)

    private Rigidbody rb; // Reference to Rigidbody to control gravity
    public bool isCooking = false; // Flag to prevent multiple objects from being cooked simultaneously

    private void Start()
    {
        timer = GetComponent<TimerUI>();
        timer.Duration = maxCookingTime;
        timer.ResetUI();
        timer.ChangeVisiblity();
        meatRenderer = GetComponentInChildren<Renderer>(); // Get the renderer
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        if (meatRenderer != null && uncookedMaterial != null)
        {
            meatRenderer.material = uncookedMaterial; // Start with uncooked material
        }

        if (rb != null)
        {
            rb.useGravity = true; // Disable gravity at the start
        }
    }

    private void Update()
    {
        if (currentState == CookState.Cooking && isCooking)
        {
            // Update cooking timer
            cookingTimer += Time.deltaTime;
            cookingProgress = Mathf.Clamp01(cookingTimer / maxCookingTime); // Normalize cooking progress (0 to 1)

            // Lerp the material based on cooking progress
            LerpMaterial(cookingProgress);

            if (cookingTimer >= maxCookingTime)
            {
                currentState = CookState.Cooked;
                cookingProgress = 1f; // Ensure it's fully cooked
                OnCooked();
            }
        }
    }

    // Function to smoothly lerp between uncooked and cooked materials
    private void LerpMaterial(float progress)
    {
        if (meatRenderer != null)
        {
            // Lerp between uncooked and cooked materials based on cooking progress
            meatRenderer.material.Lerp(uncookedMaterial, cookedMaterial, progress);
        }
    }

    // Called when cooking is complete
    private void OnCooked()
    {
        isCooking = false; // Stop the cooking process
        rb.useGravity = true; // Re-enable gravity after cooking
        meatRenderer.material = cookedMaterial;
        SetCollisionsEnabled(true);
        Debug.Log("Meat is fully cooked!");
    }

    // Start cooking with additional time
    [ContextMenu("startCooking")]
    public void debugCook()
    {
        StartCooking(1);
    }
    public void StartCooking(float extraCookingTime)
    {
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        timer.ChangeVisiblity();
        timer.Begin();
        Debug.Log("cooking");
        if (currentState == CookState.Uncooked && !isCooking) // Prevent cooking if already in progress
        {
            currentState = CookState.Cooking;
            isCooking = true; // Set flag to prevent multiple cooking
            cookingTimer = 0f;
            maxCookingTime += extraCookingTime; // Increase the cooking time if affected by the grill

            if (rb != null)
            {
                rb.useGravity = false; // Disable gravity while cooking
            }
        }
    }

    // Enable or disable collisions for the meat object
    public void SetCollisionsEnabled(bool enabled)
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = enabled;
        }
    }
}
