using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Ingredient>(out Ingredient ing))
        {
            ing.cook();
        }
        //else
        //{
        //    Debug.LogError("Not Ingredient");
        //}
    }
}
