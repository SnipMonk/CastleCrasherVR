using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeelHands : MonoBehaviour
{
    Vector3 localPos;
    Quaternion localRot;
    Vector3 localScale;
    private void Awake()
    {
        localRot = transform.localRotation;
        localPos = transform.localPosition;
    }

    public void BackOnTheWheel()
    {
        transform.localPosition = localPos;
        transform.localRotation = localRot;
    }
}
