using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
struct ToolAxisesArray
{
    public bool UPWARD;
    public bool DOWNWARD;
    public bool LEFTWARD;
    public bool RIGHTWARD;
}

public struct MovementChecker
{
    private Vector3 currentPosition, nextPosition;

    public void SetCurrentAndNext(Vector3 _current, Vector3 _next)
    {
        currentPosition = _current;
        nextPosition = _next;
    }

    public bool IsLeft => currentPosition.x > nextPosition.x;
    public bool IsRight => currentPosition.x < nextPosition.x;
    public bool IsUp => currentPosition.y < nextPosition.y;
    public bool IsDown => currentPosition.y > nextPosition.y;
}

public class TM_Tool : MonoBehaviour
{
    public event Action onCorrectMovementDone;

    [SerializeField] float pointRetrievingDelay = .1f;
    [SerializeField] int pointMaxRetrieving = 10;

    List<Vector3> points = new List<Vector3>();
    Vector3 prevPoint = Vector3.zero;
    
    MovementChecker movementChecker;
    
    [SerializeField] ToolAxisesArray axisesArray;

    void Start()
    {
        StartCoroutine(nameof(RetrieveMovementPoints));

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
                CheckToolMovement();
                ClearPoints();
            }
            else
            {
                Vector3 _p = transform.position;
                if (_p != prevPoint)
                {
                    points.Add(_p);
                }
            }

            yield return new WaitForSeconds(pointRetrievingDelay);
        }
    }

    void CheckToolMovement()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 _currentP = points[i], _nextP = points[i + 1];

            if (!CheckIfPointIsInMovement(_currentP, _nextP))
            {
                return;
            }
        }

        onCorrectMovementDone?.Invoke();
    }

    bool CheckIfPointIsInMovement(Vector3 _currentPos, Vector3 _nextPos)
    {
        movementChecker.SetCurrentAndNext(_currentPos,_nextPos);

        // if (axisesArray.DOWNWARD)
        // {
        //     if (_mc.IsUp && !axisesArray.UPWARD)
        //     {
        //         return false;
        //     }
        // }
        //
        // if (axisesArray.UPWARD)
        // {
        //     if (_mc.IsDown && !axisesArray.DOWNWARD)
        //     {
        //         return false;
        //     }
        // }
        //
        // if (axisesArray.RIGHTWARD)
        // {
        //     if (_mc.IsLeft && !axisesArray.LEFTWARD)
        //     {
        //         return false;
        //     }
        // }
        //
        // if (axisesArray.LEFTWARD)
        // {
        //     if (_mc.IsRight && !axisesArray.RIGHTWARD)
        //     {
        //         return false;
        //     }
        // }
        // return true;


        // if (_mc.IsUp && axisesArray.UPWARD)
        // {
        //     return true;
        // }
        // if (_mc.IsDown && axisesArray.DOWNWARD)
        // {
        //     return true;
        // }
        // if (_mc.IsRight && axisesArray.RIGHTWARD)
        // {
        //     return true;
        // }
        // if (_mc.IsLeft && axisesArray.LEFTWARD)
        // {
        //     return true;
        // }
        //
        // return false;

        return (movementChecker.IsUp && axisesArray.UPWARD) ||
               (movementChecker.IsDown && axisesArray.DOWNWARD) ||
               (movementChecker.IsRight && axisesArray.RIGHTWARD) ||
               (movementChecker.IsLeft && axisesArray.LEFTWARD);
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