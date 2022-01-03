using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float offsetX = 8;
    [SerializeField]
    private float offsetY = 4;

    private ParticleSystem motor;
    #endregion
    #region Unity methods
    void Awake()
    {
        motor = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
        transform.Translate(Vector3.up * vertical * speed * Time.deltaTime);

        if (transform.position.x < -offsetX) transform.position = new Vector3(-offsetX, transform.position.y);
        if (transform.position.x > offsetX) transform.position = new Vector3(offsetX, transform.position.y);
        if (transform.position.y < -offsetY) transform.position = new Vector3(transform.position.x, -offsetY);
        if (transform.position.y > offsetY) transform.position = new Vector3(transform.position.x, offsetY);
        PlayMotorAnimation(vertical, horizontal);
    }
    #endregion

    #region Methods
    private void PlayMotorAnimation(float vertical, float horizontal)
    {
        if (vertical != 0 ||
            horizontal != 0)
        {
            motor.Play();
        }
        else
        {
            motor.Stop();
        }
    }
    #endregion
}
