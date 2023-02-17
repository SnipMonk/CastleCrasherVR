using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMoveScript : MonoBehaviour
{
    [SerializeField] HingeJoint hinge;
    [SerializeField] Transform linearComp;
    [SerializeField] Transform pointrer;

    [SerializeField] float minXCord;
    [SerializeField] float maxXCord;

    [SerializeField] float prog;

    private float minAngle;
    private float maxAngle;
    private JointSpring hingeJS;
     
    private void Start()
    {
        minAngle = hinge.limits.min;
        maxAngle = hinge.limits.max;
        hingeJS = hinge.spring;
    }
    private void Update()
    {

        float newX = Mathf.Min(
                    Mathf.Max(
                        Vector3.Project(pointrer.localPosition, linearComp.right).x,
                        minXCord),
                    maxXCord
                );
        float prec = 1 - (newX - minXCord ) / (maxXCord - minXCord) ;
        

        hingeJS.targetPosition = Mathf.Lerp(minAngle, maxAngle, prec);
        hinge.spring = hingeJS;
        prog = Mathf.Lerp(minAngle, maxAngle, prec);
    }

}
