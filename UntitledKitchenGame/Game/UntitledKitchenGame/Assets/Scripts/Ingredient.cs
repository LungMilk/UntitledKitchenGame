using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] Material UNcookedMaterial;  // Reference to the cooked food material
    [SerializeField] Material cookedMaterial;  // Reference to the cooked food material
    [SerializeField] float delayBeforeSwitch = 10f; // Delay before switching to the cooked food texture
    public FoodItem fooditem;

    private Renderer[] foodRenderer; // Reference to the Renderer component of the food

    bool iscooked;

    void Awake()
    {
        try
        {
            foodRenderer = GetComponentsInChildren<Renderer>();
            changeMatierial(UNcookedMaterial);
        }
        catch (SystemException e)
        {
            Debug.LogError("can't find render\n" + e.Message);
        }

    }
    void changeMatierial(Material material)
    {
        foreach (var render in foodRenderer)
        {
            render.material = material;
        }
    }

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

    }

}