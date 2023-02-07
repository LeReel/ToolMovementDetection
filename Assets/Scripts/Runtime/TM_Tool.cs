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

        onCorrectMovementDone += ToolBehaviour;
    }

    private void Update()
    {
        Debug.Log($"{points.Count}");
    }

    IEnumerator RetrieveMovementPoints()
    {
        for (;;)
        {
            if (points.Count > pointMaxRetrieving)
            {
                ClearPoints();
            }
            else
            {
                IsDoingMovement("UPWARD");
                IsDoingMovement("DOWNWARD");
                IsDoingMovement("RIGHTWARD");
                IsDoingMovement("LEFTWARD");
                //if (_p != prevPoint)
                //{
                //    
                //    else
                //    {
                //        points.Add(_p);
                //        prevPoint = _p;
                //    }
                //}
            }

            yield return new WaitForSeconds(pointRetrievingDelay);
        }
    }

    IEnumerator CheckRotation()
    {
        for (;;)
        {
            yield return new WaitForSeconds(rotationCheckDelay);
        }
    }

    bool IsDoingMovement(string _axis)
    {
        Vector3 _p = transform.position;
        if(_p != prevPoint)
        {
            switch (_axis)
            {
                case "UPWARD":
                    if(axisesArray.UPWARD)
                    {
                        if (_p.y < prevPoint.y)
                        {
                            ClearPoints();
                        }
                    }
                    break;
                case "DOWNWARD":
                    if(axisesArray.DOWNWARD)
                    {
                        if (_p.y > prevPoint.y)
                        {
                            ClearPoints();
                        }
                    }
                    break;
                case "LEFTWARD":
                    if(axisesArray.LEFTWARD)
                    {
                        if (_p.x > prevPoint.x)
                        {
                            ClearPoints();
                        }
                    }
                    break;
                case "RIGHTWARD":
                    if(axisesArray.RIGHTWARD)
                    {
                        if (_p.x < prevPoint.x)
                        {
                            ClearPoints();
                        }
                    }
                    break;
            }
        }

        ClearPoints();
        return false;
    }

    void ClearPoints()
    {
        points.Clear();
        prevPoint = transform.position;
    }

    void ToolBehaviour()
    {
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