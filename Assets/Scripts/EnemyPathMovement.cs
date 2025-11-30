using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPathMovement : MonoBehaviour
{
    public Vector3[] points;
    public float speed = 3;

    private int currentPoint = 1;
    private int numPoints = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numPoints = points.Length;
        transform.position = points[0];
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, points[currentPoint], step);

        if (Vector3.Distance(transform.position, points[currentPoint]) < 0.001f)
        {
            // Reset the target position to the original object position.
            currentPoint = (currentPoint + 1) % numPoints;
        }
    }

	void OnDrawGizmos()
	{
        for(int i = 0; i < numPoints - 1; i++)
        {
            Gizmos.DrawLine(points[i], points[i+1]);
		}
	}
}
