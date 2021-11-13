using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    public float speed;

    private Gyroscope giro;

    //Unity documents
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;

    private float lowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        giro = Input.gyro;

        if (!giro.enabled)
        {
            giro.enabled = true;
        }

        //Unity doc
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        lowPassValue = Input.acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        //float vertical = Input.GetAxis("Vertical");
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = -Input.gyro.rotationRateUnbiased.x;
        //float horizontal = Input.gyro.rotationRateUnbiased.y;
        //float vertical = -Input.gyro.userAcceleration.x;
        //float horizontal = Input.gyro.userAcceleration.y;
        lowPassValue = LowPassFilterAccelerometer(lowPassValue);
        float vertical = -lowPassValue.z;
        float horizontal = lowPassValue.x;
        Debug.Log("Vertical: " + vertical);
        Debug.Log("Horizontal: " + horizontal);
        body.AddForce(Vector3.forward * vertical * speed);
        body.AddForce(Vector3.right * horizontal * speed*2);

        
    }

    Vector3 LowPassFilterAccelerometer(Vector3 prevValue)
    {
        Vector3 newValue = Vector3.Lerp(prevValue, Input.acceleration, lowPassFilterFactor);
        return newValue;
    }
}
