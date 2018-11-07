using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleOffset : MonoBehaviour
{
    protected void OnEnable()
    {
        GetComponent<Animator>().SetFloat("Offset", Random.Range(0.0f, 1.0f));
    }
}
