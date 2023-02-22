using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WheelScript : MonoBehaviour
{

    [SerializeField] float maxRot = 720;
    [SerializeField] float progres = 0f;
    [SerializeField] float radius = 0.2f;
    [SerializeField] Transform mover;
    [SerializeField] Transform pointer;

    [SerializeField]  CatapultShooter cs;

    [SerializeField] float prevAngle;

    [SerializeField] HingeJoint joint;

    [SerializeField] float del;
    private JointSpring spring;

    public bool Upd = false;
    int handsCounter = 0;

    public void EnableUpd()
    {
        Upd = true;
        handsCounter++;
    }

    public void DisableUpd()
    {
        if (--handsCounter == 0)
        {
            Upd = false;
        }
    }


    private void Start()
    {
        spring = joint.spring;
        prevAngle = calculateAngl();
    }

    public void Reset()
    {
        progres = 0f;
    }

    private void Update()
    {
        if (Upd)
        {

            spring.targetPosition = spring.targetPosition + 1;
            joint.spring = spring;
            mover.localPosition = projectedPoint();

            del = calcDelta();
            progres = Mathf.Max(0,
                Mathf.Min(
                    progres + del,
                    maxRot
                )
            );


            cs.GetProgress(progres / maxRot);

            spring.targetPosition = prevAngle - 180;
            joint.spring = spring;

            prevAngle = calculateAngl();
        }
    }

    private float calcDelta()
    {
        float tempAngl = calculateAngl();
        float delta = tempAngl - prevAngle;
        if(delta > 180)
        {
            delta -= 360;
        }
        else if(delta < -180)
        {
            delta += 360;
        }

        prevAngle = tempAngl;

        return delta;
    }

    private float calculateAngl()
    {
        return Mathf.Atan(mover.localPosition.x / mover.localPosition.z) * 180 / Mathf.PI + 90 + (mover.localPosition.z > 0 ? 0 : 180);
    }

    Vector3 projectedPoint()
    {
        Vector3 localPos = transform.InverseTransformPoint(pointer.position);

        localPos.y = 0;

        localPos = localPos.normalized * radius;


        return localPos;
    }
}
