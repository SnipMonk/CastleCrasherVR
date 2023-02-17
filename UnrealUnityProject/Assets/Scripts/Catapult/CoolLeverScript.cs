using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolLeverScript : MonoBehaviour
{
    [SerializeField] Transform lever_target;

    float angle = 0;
    [SerializeField] float speed = 22;
    private void Update()
    {
        //transform.localRotation = Quaternion.Euler(Vector3.up * angle);

        //angle += Time.deltaTime*speed;
        transform.LookAt(lever_target);
        //print(transform.localEulerAngles.x);
        Vector3 temp = new Vector3(transform.localEulerAngles.x, -90, 0);
        //transform.localEulerAngles = temp;
        //transform.localRotation = Quaternion.Euler(temp);
    }
}
