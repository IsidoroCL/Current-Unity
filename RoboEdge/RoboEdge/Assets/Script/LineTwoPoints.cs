using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTwoPoints : MonoBehaviour
{
    private LineRenderer lr;
    private GameObject player;
    private GameObject cross;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cross = GameObject.FindGameObjectWithTag("Cross");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //lr.SetPosition(0, player.transform.position);
            //lr.SetPosition(1, cross.transform.position);
            transform.LookAt(cross.transform);
        } 
    }
}
