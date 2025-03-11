using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //score manager just has to store and be referenced by anything that wants to increase the score
    //delegate function??? but lets keep it strict object reference.
    public float score;
    public float quota;
}
