using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformRB : MonoBehaviour
{
    [SerializeField] Transform[] movePoints;
    [SerializeField] float platformSpeed = 1f;
    int currentPoint = 0;
    bool backtrack;

    #region Editor Tools
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawPoints();
        DrawPaths();
    }

    void DrawPaths()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < movePoints.Length - 1; i++)
        {
            if (movePoints[i] != null && movePoints[i+1] != null)
            {
                Gizmos.DrawLine(movePoints[i].position, movePoints[i + 1].position);
            }
        }
    }

    void DrawPoints()
    {
        Gizmos.color = Color.magenta;
        foreach (Transform point in movePoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, .25f);
        }
    }
#endif
    #endregion

    private void FixedUpdate()
    {
        if (backtrack == false)
        {
            if (Vector3.Distance(transform.position, movePoints[currentPoint].transform.position) < .1f)
            {
                currentPoint++;

                if (currentPoint >= movePoints.Length - 1)
                {
                    backtrack = true;
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, movePoints[currentPoint].transform.position) < .1f)
            {
                currentPoint--;

                if (currentPoint == 0)
                {
                    backtrack = false;
                }
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoints[currentPoint].transform.position, platformSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.parent = gameObject.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
