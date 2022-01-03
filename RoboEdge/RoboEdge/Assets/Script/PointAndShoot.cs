using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    #region Fields
    public GameObject crosshairs;
    public GameObject player;
    public ObjectPooler pool;
    private Vector3 target;
    private Animator anim;
    private AudioSource shootSound;

    private float shootAccumulated;
    #endregion
    #region Unity methods
    private void Awake()
    {
        pool = GetComponent<ObjectPooler>();
        shootSound = GetComponent<AudioSource>();
        shootAccumulated = 1f;
    }
    void Start()
    {
        Cursor.visible = false;
        anim = player.GetComponentInChildren<Animator>();
    }
    void Update()
    {
        target = -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

        //X and Y coordenates are multipled by 2 because is the limit of the screen in perspective camera
        crosshairs.transform.position = new Vector3(target.x * 2, target.y * 2, crosshairs.transform.position.z);
        if (player != null)
        {
            if (Input.GetButton("Fire1"))
            {
                shootAccumulated += Time.deltaTime;
                if (shootAccumulated > 5) shootAccumulated = 5f;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                FireBullet();
                shootAccumulated = 1f;
            }
        }
    }
    #endregion
    #region Methods
    void FireBullet()
    {
        anim.SetTrigger("Shooting");
        shootSound.Play();
        GameObject bullet = pool.GetPooledObject() as GameObject;
        bullet.GetComponent<Bullet>().SetConfiguration(player.transform.position, new Vector3(0.4f, 0.4f, 0.4f), shootAccumulated);
    }
    #endregion
}
