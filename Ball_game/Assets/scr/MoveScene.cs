using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScene : MonoBehaviour
{
    public int sens; //Valor de sensibilidad al movimiento del móvil
    public float correcion; //Valor para mostrar vertical la pantalla del móvil
    Gyroscope giro;
    void Start()
    {

        giro = Input.gyro;

        if (!giro.enabled)
        {
            giro.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //transform.rotation *= Quaternion.Euler(Input.acceleration.y / 6, 0, -Input.acceleration.x / 3);
        transform.rotation = Quaternion.Euler((Input.gyro.attitude.x) *sens, 0, (-Input.gyro.attitude.y) * sens);
        print(giro.attitude.x);
        
        //transform.rotation *= Quaternion.Euler(Input.GetAxis("Horizontal") / sens, Input.GetAxis("Vertical") / sens, 0);
    }
}
