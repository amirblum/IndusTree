using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MidpointCalculator : MonoBehaviour
{
    public GameObject PlayerOne;
    public GameObject PlayerTwo;

    public Vector3 AvgBetweenTwoV3 ()
    {
        float[] Xs = new float[2] { PlayerOne.transform.position.x, PlayerTwo.transform.position.x };
        float[] Zs = new float[2] { PlayerOne.transform.position.z, PlayerTwo.transform.position.z };
        float Xavg = (Xs.Max() + Xs.Min()) / 2;
        float Zavg = (Zs.Max() + Zs.Min()) / 2;
        return new Vector3(Xavg, 0.0f, Zavg);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = AvgBetweenTwoV3();
    }
}
