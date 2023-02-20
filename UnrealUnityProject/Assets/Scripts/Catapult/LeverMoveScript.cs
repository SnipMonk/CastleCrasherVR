using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMoveScript : MonoBehaviour
{
    [SerializeField] HingeJoint hinge;
    [SerializeField] Transform linearComp;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform pointrer;

    [SerializeField] float minXCord;
    [SerializeField] float maxXCord;

    [SerializeField] float prog;

    [SerializeField] Vector3 debug;
    private float minAngle;
    private float maxAngle;
    private JointSpring hingeJS;
     
    private void Start()
    {
        minAngle = hinge.limits.min;
        maxAngle = hinge.limits.max;
        hingeJS = hinge.spring;
    }

    private Vector3 projectedPont(Vector3 pos)
    {
        return Vector3.Project(pointrer.position - linearComp.position, linearComp.right) + linearComp.position;
    }

    private float clampedDist()
    {
        Vector3 projcted = projectedPont(pointrer.position);
        Vector3 local = linearComp.InverseTransformPoint(projcted);

        float dist = (local).magnitude;
        dist *= Mathf.Sign(Vector3.Dot(Vector3.right, local));

        debug = Vector3.one * dist;

        dist = Mathf.Max(
            Mathf.Min(
                dist,
                maxXCord
                ),
            minXCord
        );

        return dist;
    }

    private Vector3 clampPos()
    {

        Vector3 projcted = projectedPont(pointrer.position);
        Vector3 local = linearComp.InverseTransformPoint(projcted);

        local = Vector3.right * clampedDist();
        return local;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        
        Gizmos.DrawSphere(linearComp.position, 0.13f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pointrer.position, 0.12f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(projectedPont(pointrer.position), 0.1f);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(linearComp.TransformPoint(clampPos()), 0.2f) ;
    }

    private void Update()
    {
        prog = 1 - (clampedDist() - minXCord) / (maxXCord - minXCord);
        hingeJS.targetPosition = Mathf.Lerp(minAngle, maxAngle, prog);
        hinge.spring = hingeJS;
    }

    public void ResetPointer()
    {
        pointrer.position = endPoint.position;
        pointrer.rotation = endPoint.rotation;
        // Vector3 projected = projectedPont(linearComp.InverseTransformPoint(pointrer.position));
        // projected.x = calcNewX() - minXCord;
        // pointrer.position = linearComp.TransformPoint(projected);
    }

}
