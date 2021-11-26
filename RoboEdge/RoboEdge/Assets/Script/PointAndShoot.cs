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
    #endregion
    #region Unity methods
    private void Awake()
    {
        pool = GetComponent<ObjectPooler>();
        shootSound = GetComponent<AudioSource>();
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
        crosshairs.transform.position = new Vector3(target.x *2, target.y *2, crosshairs.transform.position.z);
        if (player != null)
        {
            if (Input.GetButtonDown("Fire1")) FireBullet();
        }  
    }
    #endregion
    #region Methods
    void FireBullet()
    {
        anim.SetTrigger("Shooting");
        shootSound.Play();
        GameObject b = pool.GetPooledObject() as GameObject;
        b.transform.position = player.transform.position;
        b.SetActive(true);
    }
    #endregion
}
