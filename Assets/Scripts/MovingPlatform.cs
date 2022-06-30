using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform[] movePoints;
    [SerializeField] float platformSpeed = 1f;
    int currentPoint = 0;
    bool backtrack;

    #region Editor Visuals
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawPoints();
        DrawPaths();
    }

    private void DrawPoints()
    {
        Gizmos.color = Color.magenta;
        foreach (Transform point in movePoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, .25f);
        }
    }

    private void DrawPaths()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < movePoints.Length - 1; i++)
        {
            if (movePoints[i] != null && movePoints[i+1] != null)
            {
                Vector3 thisPoint = movePoints[i].position;
                Vector3 nextPoint = movePoints[i + 1].position;
                Gizmos.DrawLine(thisPoint, nextPoint);
            }
        }
    }
#endif
    #endregion

    void FixedUpdate()
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

        transform.position = Vector3.MoveTowards(transform.position, movePoints[currentPoint].transform.position, platformSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.parent.gameObject.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.parent.gameObject.transform.parent = null;
        }
    }
}
