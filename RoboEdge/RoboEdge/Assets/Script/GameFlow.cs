using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    private GameObject player;
    public static GameFlow sharedInstance;

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
    private void Awake()
    {
        sharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Waves());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Waves()
    {
        //This method control the waves of enemies in the game

        Wave1();
        yield return new WaitForSeconds(10);
        Wave2();
        yield return new WaitForSeconds(15);
        Wave1();
        yield return new WaitForSeconds(15);
        Wave3(1f, 0f);
        Wave1();
        yield return new WaitForSeconds(15);
        Wave4();
        yield return new WaitForSeconds(25);
        Wave5();
        Wave3(0f, 0f);
        yield return new WaitForSeconds(50);
        Wave6();
        yield return new WaitForSeconds(25);
        Wave2();
        Wave3(-5f, 0f);
        yield return new WaitForSeconds(15);
        Wave6();
        yield return new WaitForSeconds(60);

        Wave3(0f, 0f);
        yield return new WaitForSeconds(5);
        Wave3(3f, 0f);
        yield return new WaitForSeconds(5);
        Wave3(-3f, 0f);
        yield return new WaitForSeconds(5);
        Wave3(0f, 3f);
        yield return new WaitForSeconds(20);
        Wave3(0f, -3f);
        Wave1();
        yield return new WaitForSeconds(10);
        Wave2();
        yield return new WaitForSeconds(15);
        Wave1();
        yield return new WaitForSeconds(30);        
        Wave9();
        yield return new WaitForSeconds(5);
        Wave9();
        yield return new WaitForSeconds(5);
        Wave9();
        yield return new WaitForSeconds(5);
        Wave9();
        yield return new WaitForSeconds(5);
        Wave9();
        yield return new WaitForSeconds(5);
        Wave9();
        yield return new WaitForSeconds(5);

    }

    void Wave1()
    {
        Instantiate(enemies[0], new Vector3(4, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-4, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, 4, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, -4, instantiateDistance), Quaternion.identity);
    }

    void Wave2()
    {
        Instantiate(enemies[2], new Vector3(4, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[2], new Vector3(-4, 0, instantiateDistance), Quaternion.identity);
    }

    void Wave3(float x, float y)
    {
        Instantiate(enemies[1], new Vector3(x, y, instantiateDistance), Quaternion.identity);
    }

    void Wave4()
    {
        Instantiate(enemies[0], new Vector3(4, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(2, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-2, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-4, -2, instantiateDistance), Quaternion.identity);
    }

    void Wave5()
    {
        Instantiate(enemies[0], new Vector3(4, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(2, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(0, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-2, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[0], new Vector3(-4, 2, instantiateDistance), Quaternion.identity);
    }

    void Wave6()
    {
        Instantiate(enemies[3], new Vector3(-30, -4, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-35, -2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-40, 0, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-35, 2, instantiateDistance), Quaternion.identity);
        Instantiate(enemies[3], new Vector3(-30, 4, instantiateDistance), Quaternion.identity);
    }

    void Wave7()
    {
        Instantiate(enemies[5], new Vector3(-17, -24, instantiateDistance +100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(0, 0, instantiateDistance +100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-9, 13, instantiateDistance +100), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(0, 16, instantiateDistance +100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-11, 25, instantiateDistance +105), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(14, 3, instantiateDistance +103), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(-14, 13, instantiateDistance +118), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(2, 22, instantiateDistance +113), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-21, -15, instantiateDistance +112), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(32, -16, instantiateDistance + 100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(11, 21, instantiateDistance + 100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(19, -23, instantiateDistance + 100), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(1, 20, instantiateDistance + 110), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(32, -15, instantiateDistance + 105), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-14, -34, instantiateDistance + 103), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(-12, -44, instantiateDistance + 118), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(20, 13, instantiateDistance + 113), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-23, 51, instantiateDistance + 112), Quaternion.identity);
    }

    void Wave8()
    {
        Instantiate(enemies[5], new Vector3(12, -16, instantiateDistance+100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(11, 51, instantiateDistance+100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(19, -13, instantiateDistance+100), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(1, 10, instantiateDistance + 110), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(12, -15, instantiateDistance + 105), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-14, -14, instantiateDistance + 103), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(-12, -24, instantiateDistance + 118), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(20, 13, instantiateDistance + 113), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-23, 1, instantiateDistance + 112), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(32, -16, instantiateDistance + 100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(11, 31, instantiateDistance + 100), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(19, -33, instantiateDistance + 100), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(1, 30, instantiateDistance + 110), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(22, -55, instantiateDistance + 105), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-34, -14, instantiateDistance + 103), Quaternion.identity);

        Instantiate(enemies[5], new Vector3(-12, -24, instantiateDistance + 118), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(30, 33, instantiateDistance + 113), Quaternion.identity);
        Instantiate(enemies[5], new Vector3(-23, 1, instantiateDistance + 112), Quaternion.identity);
    }

    void Wave9()
    {
        Instantiate(enemies[4], new Vector3(40, 0, 10), Quaternion.identity);
    }


    public void CallGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        //IMPLEMENTAR
        Debug.Log("Muerto");
        Destroy(player);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
