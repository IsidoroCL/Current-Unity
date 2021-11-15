using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : MonoBehaviour
{
    public Vector2 positionToMove;
    public float duration;
    
    // Start is called before the first frame update
    void Start()
    {
        positionToMove = new Vector2(-6.0f, -1.5f);
        StartCoroutine(LerpPosition(positionToMove, duration));
    }

    // Update is called once per frame
    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;

        while(time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
