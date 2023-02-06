using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TM_Tool : MonoBehaviour
{
    float pointRetrievingDelay = .5f, rotationCheckDelay = 1;
    int pointMaxRetrieving = 10;

    List<Vector3> points = new List<Vector3>();


    void Start()
    {
        StartCoroutine(nameof(RetrieveMovementPoints));
        StartCoroutine(nameof(CheckRotation));
    }

    private void Update()
    {
        Debug.Log($"{points.Count}");
    }

    IEnumerator RetrieveMovementPoints()
    {
        for(;;)
        {
            if (points.Count > pointMaxRetrieving)
            {
                points.Clear();
            }
            else
            {
                points.Add(transform.position);
            }
            yield return new WaitForSeconds(pointRetrievingDelay);
        }
    }

    IEnumerator CheckRotation()
    {
        for(;;)
        {
            yield return new WaitForSeconds(rotationCheckDelay);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        int _max = points.Count;
        for (int i = 0; i < _max; i++)
        {
            {
                Gizmos.DrawSphere(points[i], .3f);
            }
        }
    }
}