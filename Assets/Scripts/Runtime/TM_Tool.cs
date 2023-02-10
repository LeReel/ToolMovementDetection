using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public struct ToolAxisesArray
{
    [SerializeField] public bool UPWARD;
    [SerializeField] public bool DOWNWARD;
    [SerializeField] public bool LEFTWARD;
    [SerializeField] public bool RIGHTWARD;

    public ToolAxisesArray(bool _value)
    {
        UPWARD = DOWNWARD = LEFTWARD = RIGHTWARD = _value;
    }
}

public class TM_Tool : MonoBehaviour
{
    public event Action onCorrectMovementDone;

    float pointRetrievingDelay = .1f, rotationCheckDelay = 1;
    int pointMaxRetrieving = 10;


    List<Vector3> points = new List<Vector3>();
    Vector3 prevPoint = Vector3.zero;

    [SerializeField] ToolAxisesArray axisesArray = new ToolAxisesArray(false);

    void Start()
    {
        StartCoroutine(nameof(RetrieveMovementPoints));
        StartCoroutine(nameof(CheckRotation));

        onCorrectMovementDone += () =>
        {
            points.Clear();
            ToolBehaviour();
        };
    }

    IEnumerator RetrieveMovementPoints()
    {
        for (;;)
        {
            if (points.Count > pointMaxRetrieving)
            {
                IsDoingMovement();
                ClearPoints();
            }
            else
            {
                Vector3 _p = transform.position;
                if (_p != prevPoint)
                {
                    points.Add(_p);
                    CheckIfPointIsInMovement();
                }
            }

            yield return new WaitForSeconds(pointRetrievingDelay);
        }
    }

    void CheckIfPointIsInMovement()
    {
    }

    IEnumerator CheckRotation()
    {
        for (;;)
        {
            yield return new WaitForSeconds(rotationCheckDelay);
        }
    }

    void IsDoingMovement()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 _currentP = points[i], _nextP = points[i + 1];

            if (axisesArray.UPWARD)
            {
                if (_currentP.y > _nextP.y && !axisesArray.DOWNWARD)
                {
                    return;
                }
            }
            if (axisesArray.DOWNWARD)
            {
                if (_currentP.y < _nextP.y && !axisesArray.UPWARD)
                {
                    return;
                }
            }

            if (axisesArray.LEFTWARD)
            {
                if (_currentP.x < _nextP.x && !axisesArray.RIGHTWARD)
                {
                    return;
                }
            }
            if (axisesArray.RIGHTWARD)
            {
                if (_currentP.x > _nextP.x && !axisesArray.LEFTWARD)
                {
                    return;
                }
            }
        }
        
        onCorrectMovementDone?.Invoke();
    }

    void ClearPoints()
    {
        points.Clear();
        prevPoint = transform.position;
    }

    void ToolBehaviour()
    {
        Debug.Log("Tool Behaviour");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        int _max = points.Count;
        for (int i = 0; i < _max; i++)
        {
            {
                Gizmos.DrawSphere(points[i], .1f);
            }
        }
    }
}