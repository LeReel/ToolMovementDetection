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

    [Header("Tool Behaviour")] [SerializeField]
    protected float sphereCastRadius = 5;

    [SerializeField] protected LayerMask hitLayers;

    [Header("Movement Checking")] [SerializeField]
    protected float pointRetrievingDelay = .1f;

    [SerializeField] protected int pointMaxRetrieving = 10, pointAmountBeforeTriggering = 5;
    [SerializeField] ToolAxisesArray axisesArray;

    protected List<Vector3> points = new List<Vector3>();
    protected Vector3 prevPoint = Vector3.zero;

    protected MovementChecker movementChecker;

    void Start()
    {
        StartCoroutine(nameof(RetrieveMovementPoints));
        
        onCorrectMovementDone += () =>
        {
            RaycastHit _hit;
            bool _hasHit = Physics.SphereCast(
                transform.position,
                sphereCastRadius,
                transform.forward,
                out _hit,
                1,
                hitLayers,
                QueryTriggerInteraction.UseGlobal);
        
            if (_hasHit)
            {
                ToolBehaviour();
            }
        
            points.Clear();
        };
    }

    IEnumerator RetrieveMovementPoints()
    {
        for (;;)
        {
            Vector3 _p = transform.position;
            if (_p != prevPoint)
            {
                prevPoint = _p;
                points.Add(_p);
                if (points.Count > pointMaxRetrieving)
                {
                    points.Clear();
                }
                else if (points.Count > pointAmountBeforeTriggering)
                {
                    CheckToolMovement();
                }
            }
            else
            {
                if (points.Count > 0 && points.Count > pointAmountBeforeTriggering)
                {
                    //CheckToolMovement();
                    points.Clear();
                }
            }

            yield return new WaitForSeconds(pointRetrievingDelay);
        }
    }

    void CheckToolMovement()
    {
        for (int i = 0, j = points.Count - 1; j > 0; i++, j--)
        {
            Vector3 _currentP = points[j - 1], _nextP = points[j];

            if (!CheckIfPointIsInMovement(_currentP, _nextP))
            {
                return;
            }

            if (i > pointAmountBeforeTriggering)
            {
                onCorrectMovementDone?.Invoke();
                break;
            }
        }
    }

    bool CheckIfPointIsInMovement(Vector3 _currentPos, Vector3 _nextPos)
    {
        movementChecker.SetCurrentAndNext(_currentPos, _nextPos);

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereCastRadius);
    }
}