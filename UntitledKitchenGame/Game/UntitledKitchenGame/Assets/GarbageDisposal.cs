using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageDisposal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //print("object entered garbage");
        if(other.gameObject.tag == "Food")
        {
            Destroy(other.gameObject);
        }
    }
}
