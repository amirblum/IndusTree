using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownManager : MonoBehaviour
{

    public static CoolDownManager Instance;
    public float CoolDownTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator StartCooldDown() 
    {
        yield return new WaitForSeconds(CoolDownTimer);
        transform.GetComponent<PlayerController>().IsPlacementLocked = false;
        yield break;
        
    }

    //public IEnumerator SetPlacementLock ()
    //{
    //    transform.GetComponent<PlayerController>().IsPlacementLocked = false;
    //    yield break;
    //}

    public float CountSecondsFloat()
    {
        return 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void Awake()
    {
        Instance = this;
    }

}