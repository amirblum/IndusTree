using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownManager : MonoBehaviour
{

    public static CoolDownManager Instance;
    public float CoolDownTimer;
    public GameObject PlayerCursorObject;
    public Renderer PlayerCursorRenderer;
    public float EmissionFactor;
    public Color TargetColor;
    public float CountdownTimer;
    public float CountDownTimerForColorLerp;
    public float ClampdLerpedCountdown;
    public Color InitColor;

    public void DecrementFromCoolDownTimerToZero ()
    {
        
        if (CountdownTimer >= 0)
        {
            CountdownTimer -= Time.deltaTime;
            Debug.Log("Countdown Timer is :  " + CountdownTimer);
        }
        
    }

    public void ChangeColorToHighLight ()
    {
        //PlayerCursorObject.GetComponent<Renderer>().material.color = Color.Lerp(GradientTarget, PlayerCursorObject.GetComponent<Renderer>().material.color, Mathf.PingPong(Time.time, 1));
        PlayerCursorObject.GetComponent<Renderer>().material.color = new Color(TargetColor.r, TargetColor.g, TargetColor.b);
        
    }

    //public void coolDownControl ()
    //{

    //}

    // Start is called before the first frame update
    void Start()
    {
        CountdownTimer = CoolDownTimer;
        //var PlayerCursorMaterial = PlayerCursorObject.GetComponent<Renderer>().material;

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
        DecrementFromCoolDownTimerToZero();
    }

    protected void Awake()
    {
        Instance = this;
    }

}