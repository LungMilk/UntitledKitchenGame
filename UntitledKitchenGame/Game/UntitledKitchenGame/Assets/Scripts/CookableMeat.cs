using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookableMeat : MonoBehaviour
{
    //this script will be attached to the object that is cookable
    //Object needs to handle its Uncooked state, cooking state and cooked state
    //object needs a cooking value and a cooking max that is represented by seconds to cook
    //object needs to be able to grab its collider and disable in the different cooked state.


    [SerializeField] Material UNcookedMaterial;  // Reference to the cooked food material
    [SerializeField] Material cookedMaterial;  // Reference to the cooked food material
    [SerializeField] float delayBeforeSwitch = 10f; // Delay before switching to the cooked food texture

    [SerializeField]
    protected float cookingValue;
    public float cookTime;

    public FoodItem fooditem;

    private Renderer[] foodRenderer; // Reference to the Renderer component of the food
    private Collider collider;
    public bool iscooked;

    public foodState state;
    public enum foodState
    {
        cooking,cooked,uncooked
    }

    private void Start()
    {
        try
        {
            foodRenderer = GetComponentsInChildren<Renderer>();
            collider = this.GetComponent<Collider>();
            changeMatierial(UNcookedMaterial);
        }
        catch (SystemException e)
        {
            Debug.LogError("can't find render\n" + e.Message);
        }
    }
    private void Update()
    {
        if (cookingValue >= cookTime)
        {
            iscooked = true;
            state = foodState.cooked;
        }

        if(state == foodState.uncooked)
        {
            Uncooked();
        }
        else if(state == foodState.cooking)
        {
            Cooking();
        }
        else if (state == foodState.cooked)
        {
            Cooked();
        }
    }
    void Uncooked()
    {
        collider.enabled = true;
        changeMatierial(UNcookedMaterial);
    }
    void Cooked()
    {
        collider.enabled = true;
        iscooked = true;
        changeMatierial(cookedMaterial);
    }
    void Cooking()
    {
        collider.enabled = false;
        cookingValue += Time.deltaTime;
        //Material.Lerp();
    }
    void changeMatierial(Material material)
    {
        foreach (var render in foodRenderer)
        {
            render.material = material;
        }
    }
}
