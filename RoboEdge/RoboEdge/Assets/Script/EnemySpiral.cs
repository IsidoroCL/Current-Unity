using UnityEngine;

public class EnemySpiral : Enemy
{
    #region Fields
    private float timeCounter;
    [SerializeField]
    private float radius;
    #endregion
    #region Unity methods
    private void Awake()
    {
        speed = 5.0f;
        life = 1;
        repeatFire = 3;
    }
    void Start()
    {
        Init();
    }

    void Update()
    {
        Move();
    }
    #endregion
    #region Methods
    protected override void Move()
    {
        if (transform.position.z > 10)
        {
            base.Move();
        }
        
        //Spiral movement
        timeCounter += Time.deltaTime;
        float x = Mathf.Cos(timeCounter);
        float y = Mathf.Sin(timeCounter);
        transform.position = new Vector3(x * radius, y * radius, transform.position.z);
    }
    #endregion
}
