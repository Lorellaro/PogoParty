using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouyancy : MonoBehaviour
{
    public Transform[] Floaters;
    public float UnderWaterDrag = 3f;
    public float UnderWaterAngularDrag = 1f;
    public float AirDrag = 0f;
    public float AirAngularDrag = 0.05f;
    public float FloatingPower = 15f;
    public float WaterHeight = 0f;
    public float waterBobTime = 1f;
    public float waterBobSpeed = 1f;
    public float straightenForce = 10f;


    float currentBobTime;
    Rigidbody Rb;
    bool Underwater;
    int FloatersUnderWater;
    bool moveWaterUp;

    // Start is called before the first frame update
    void Start()
    {
        Rb = this.GetComponent<Rigidbody>();
        StartCoroutine(adjustWaterHeight());
        StartCoroutine(adjustValue());
    }

    private IEnumerator adjustWaterHeight()
    {
        //Update loop
        while (true)
        {
            if(moveWaterUp)
            {
                WaterHeight -= waterBobSpeed * Time.deltaTime;
            }

            else
            {
                WaterHeight += waterBobSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }

    private IEnumerator adjustValue()
    {
        yield return new WaitForSeconds(waterBobTime);
        moveWaterUp = !moveWaterUp;
        StartCoroutine(adjustValue());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FloatersUnderWater = 0;

        for (int i = 0; i < Floaters.Length; i++)
        {
            float diff = Floaters[i].position.y - WaterHeight;
            if (diff < 0)
            {
                Rb.AddForceAtPosition(Vector3.up * FloatingPower * Mathf.Abs(diff), Floaters[i].position, ForceMode.Force);
                FloatersUnderWater += 1;
                if (!Underwater)
                {
                    Underwater = true;
                    SwitchState(true);
                }
            }
        }
        if (Underwater && FloatersUnderWater == 0)
        {
            Underwater = false;
            SwitchState(false);
        }

        var rot = Quaternion.FromToRotation(transform.forward, Vector3.up);
        Rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * straightenForce);
    }
    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            Rb.drag = UnderWaterDrag;
            Rb.angularDrag = UnderWaterAngularDrag;
        }
        else
        {
            Rb.drag = AirDrag;
            Rb.angularDrag = AirAngularDrag;
        }
    }
}
