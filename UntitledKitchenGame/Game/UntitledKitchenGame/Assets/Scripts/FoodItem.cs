using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewItem.asset", menuName = "Data/Food Item")]
public class FoodItem : ScriptableObject
{
    public string displayName;
    public Sprite displayImage;
    //[CustomPreview(Mesh.AcquireReadOnlyMeshData())] hopefully something works
    public GameObject foodObjectPrefab;
    //each scriptable object will have its appropriate game object attached to the scriptable object. 
    /*what this means is that the item spawner will be grabbing the related objects foodObject variable and spawning it in
     * the order submission box will have to grab the items name and sort to see if that item matches any of the order items foodObject variable
     * 
    */

    public FoodType type;

    public int pointValue;
    public int rarity;
    public int stages;

}
