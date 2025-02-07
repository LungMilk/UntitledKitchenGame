using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public ItemCollection itemDatabase;

    public List<FoodItem> foodItems;
    public int itemMax = 3;
    // Start is called before the first frame update
    [ContextMenu ("Populate Order")]
    void PopulateOrder()
    {
        for (int i = 0; i < itemMax; i++)
        {
            foodItems.Add(itemDatabase.SelectRandomItem());
        }
    }
}
