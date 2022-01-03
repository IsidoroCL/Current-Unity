using UnityEngine;

public class Planet : MonoBehaviour
{
    float timePassed;
    public float secondsScenerio;

    void Update()
    {
        timePassed += Time.deltaTime;
        if (transform.localScale.x < 1000)
        {
            transform.localScale = Vector3.Lerp(
                new Vector3(1, 1, 1),
                new Vector3(1000, 1000, 200),
                timePassed / secondsScenerio);
        }
    }
}
