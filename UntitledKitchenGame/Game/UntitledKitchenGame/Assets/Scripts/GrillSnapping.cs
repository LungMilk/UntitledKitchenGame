using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillSnapping : MonoBehaviour
{
    //grill needs to find a nearby object that is cookable,
    //grab the object and disable its colliders or put the object in the cooking state
    //lerp its position to the center transform and prevent otehr objects from being attached
    //increase its cooking value
    //once cooking value is at max enable its colliders and disengage the lock

    //need a collider that is detecting stuff via a layer mask of food objects
    public Collider detectorCollider;
    public float cookingRadius;
    public LayerMask objectsToCook;
    // Start is called before the first frame update
    void Start()
    {
        detectorCollider = this.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(this.gameObject.transform.position,cookingRadius,objectsToCook);
        if (detectedObjects[0].gameObject.GetComponent<CookableMeat>())
        {
            CookableMeat targetObj = detectedObjects[0].gameObject.GetComponent<CookableMeat>();
            targetObj.state = CookableMeat.foodState.cooking;
            LerpPosToAnchor(detectedObjects[0].gameObject.transform);
        }
    }
    void LerpPosToAnchor(Transform target)
    {
        if (target.position != this.transform.position)
        {
            float t = 0;
            t += Time.deltaTime;
            target.position = Vector3.Lerp(target.position, this.transform.position, t);
        }
    }

}
