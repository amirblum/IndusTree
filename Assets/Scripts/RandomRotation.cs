using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    protected void OnEnable()
    {
        transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
}
