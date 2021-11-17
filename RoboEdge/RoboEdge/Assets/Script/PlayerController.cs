using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float offsetX = 8;
    public float offsetY = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Move Player according to Input 
        transform.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
        transform.Translate(Vector3.up * vertical * speed * Time.deltaTime);
        
        if (transform.position.x < -offsetX) transform.position = new Vector3(-offsetX, transform.position.y);
        if (transform.position.x > offsetX) transform.position = new Vector3(offsetX, transform.position.y);
        if (transform.position.y < -offsetY) transform.position = new Vector3(transform.position.x, -offsetY);
        if (transform.position.y > offsetY) transform.position = new Vector3(transform.position.x, offsetY);

    }

    
}
