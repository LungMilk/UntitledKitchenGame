using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyDown : MonoBehaviour
{
    public Image targImg;
    public Sprite defaultKey;
    public Sprite heldKey;
    public KeyCode keybind; // Change this to your key
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(keybind))
        {
            targImg.sprite = heldKey;
        }
        else
        {
            targImg.sprite = defaultKey;
        }
    }
}
