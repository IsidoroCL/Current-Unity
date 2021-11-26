using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    #region Fields
    private GameObject player;
    private AudioSource mainMusic;
    public static GameFlow sharedInstance;
    private bool final;

    [SerializeField]
    private GameObject[] enemies;
    //0: Simple
    //1: Aimed
    //2: Spiral
    //3: ScrollX
    //4: Small
    //5: Asteroids

    [SerializeField]
    private float instantiateDistance;

    [SerializeField]
    private GameObject textWin;
    #endregion
    #region Unity methods
    private void Awake()
    {
        sharedInstance = this;
        mainMusic = GetComponent<AudioSource>();
        final = false;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainMusic.PlayDelayed(1.0f);
        StartCoroutine(Waves());
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        if (final)
        {
            //If there are not any final enemies, then player won
            EnemySmall[] enemies = GameObject.FindObjectsOfType<EnemySmall>();
            if (enemies.Length < 1) Win();
        }
    }
    #endregion
    #region Methods
    //This method control the waves of enemies in the game
    //Each wave instantiate a number of enemies
    private IEnumerator Waves()
    {
        Wave1();
        yield return new WaitForSeconds(2);
        Wave8();
        yield return new WaitForSeconds(10);
        Wave6();
        Wave2();
        yield return new WaitForSeconds(10);
        Wave8();
        Wave1();
        yield return new WaitForSeconds(15);
        Wave3(1f, 0f);
        Wave1();
        yield return new WaitForSeconds(15);
        Wave4();
        yield return new WaitForSeconds(25);
        Wave5();
        Wave3(0f, 0f);
        yield return new WaitForSeconds(30);
        Wave6();
        Wave3(-5f, 0f);
        yield return new WaitForSeconds(15);
        Wave6();
        Wave3(0f, 0f);
        yield return new WaitForSeconds(5);
        Wave3(3f, 0f);
        yield return new WaitForSeconds(5);
        Wave3(-3f, 0f);
        yield return new WaitForSeconds(4);
        Wave3(0f, 3f);
        yield return new WaitForSeconds(3);
        Wave3(3f, 0f);
        yield return new WaitForSeconds(2);
        Wave3(-3f, 0f);
        yield return new WaitForSeconds(1);
        Wave3(0f, 3f);
        yield return new WaitForSeconds(20); 
        
        //Final enemies
        for (int i = 0; i < 5; i++)
        {
            Wave7();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 5; i++)
        {
            Wave7();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 5; i++)
        {
            Wave7();
            yield return new WaitForSeconds(0.5f);
        }

        //Do you Win? this boolean allow to check if you win in Update method
        final = true;        
    }

    private void Wave1()
    {
        Instantiate(enemies[0], new Vector3(4, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-4, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, 4, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, -4, instantiateDistance), Quaternion.identity);
    }

    private void Wave2()
    {
        Instantiate(enemies[2], new Vector3(4, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[2], new Vector3(-2, 0, instantiateDistance), Quaternion.identity);
    }

    private void Wave3(float x, float y)
    {
        Instantiate(enemies[1], new Vector3(x, y, instantiateDistance), Quaternion.identity);
    }

    private void Wave4()
    {
        Instantiate(enemies[0], new Vector3(4, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(2, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-2, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-4, -2, instantiateDistance), Quaternion.identity);
    }

    private void Wave5()
    {
        Instantiate(enemies[0], new Vector3(4, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(2, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-2, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-4, 2, instantiateDistance), Quaternion.identity);
    }

    private void Wave6()
    {
        Instantiate(enemies[3], new Vector3(-30, -4, 10), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-35, -2, 10), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-40, 0, 10), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-35, 2, 10), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-30, 4, 10), Quaternion.identity);
    }

    private void Wave7()
    {
        Instantiate(enemies[4], new Vector3(40, 0, 10), Quaternion.identity);
    }

    private void Wave8()
    {
        Instantiate(enemies[3], new Vector3(-40, -4, 10), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-35, -2, 10), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-30, 0, 10), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-35, 2, 10), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-40, 4, 10), Quaternion.identity);
    }
    public void CallGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        Destroy(player);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
    private void Win()
    {
        textWin.SetActive(true);
    }
    #endregion
}
