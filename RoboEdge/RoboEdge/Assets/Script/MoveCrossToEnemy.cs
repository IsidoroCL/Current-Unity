using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCrossToEnemy : MonoBehaviour
{
    public Transform cross;
    private float interval;
    private bool hasTarget = false;

    private void Update()
    {
        if (cross.localPosition != new Vector3(0, 0, 0.1f) &&
            !hasTarget)
        {
            interval += Time.deltaTime;
            if (interval > 1.0f) interval = 1.0f;
            cross.localPosition = Vector3.Lerp(cross.localPosition,
                new Vector3(0, 0, 0.1f),
                interval);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        hasTarget = true;
        interval += Time.deltaTime;
        if (interval > 1.0f) interval = 1.0f;
        cross.localPosition = Vector3.Lerp(cross.localPosition,
            new Vector3(0, 0, other.transform.position.z / 100),
            interval);
    }

    private void OnTriggerExit(Collider other)
    {
        hasTarget = false;
        interval = 0;
    }
}
