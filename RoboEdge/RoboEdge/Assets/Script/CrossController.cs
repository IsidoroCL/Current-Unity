using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossController : MonoBehaviour
{
    public float speed = 8.0f;
    public float offsetX = 8;
    public float offsetY = 4;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("CrossHorizontal");
        float vertical = Input.GetAxis("CrossVertical");

        // Move Player according to Input 
        transform.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
        transform.Translate(Vector3.up * vertical * speed * Time.deltaTime);

        if (transform.position.x < -offsetX) transform.position = new Vector3(-offsetX, transform.position.y);
        if (transform.position.x > offsetX) transform.position = new Vector3(offsetX, transform.position.y);
        if (transform.position.y < -offsetY) transform.position = new Vector3(transform.position.x, -offsetY);
        if (transform.position.y > offsetY) transform.position = new Vector3(transform.position.x, offsetY);
    }
}
