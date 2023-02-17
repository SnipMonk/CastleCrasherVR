using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultShooter : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] GameObject projectilePref;
    private Transform curProjectile;
    [SerializeField] Transform projectileHolder;

    [Range(0f,1f)]
    [SerializeField] float progress = 0;
    [Range(0f, 1f)]
    [SerializeField] float minShootProgress = 0.7f;
    [SerializeField] float minShootForce;
    [SerializeField] float maxShootForce;
    [SerializeField] HingeJoint spoonHinge;
    [SerializeField] float maxShootTimer;
    [SerializeField] AnimationCurve shootCurve;
    [SerializeField] float shootForce = 100;

    private float upAngle;
    private float downAngle;

    private JointSpring hingeJS;
    private float hingeJSSpring;
    private bool shooting = false;

    private void Start()
    {
        upAngle = spoonHinge.limits.min;
        downAngle = spoonHinge.limits.max;
        hingeJS = spoonHinge.spring;
        hingeJSSpring = hingeJS.spring;
        reloadProjectile();
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        if (progress > minShootProgress)
        {
            shooting = true;
            StartCoroutine(ShootCoroutine());
        }
    }
    private void Update()
    {
        if (!shooting) {
            changeHingeTargetPos(progress);
        }
    }

    public float GetAngle(float percent)
    {
        return (downAngle - upAngle) * percent + upAngle;
    }

    private void changeHingeTargetPos(float pecent)
    {
        hingeJS.targetPosition = GetAngle(pecent);
        spoonHinge.spring = hingeJS;
    }

    private void throwProjectile()
    {
        if (curProjectile != null)
        {
            float force = Mathf.Lerp(minShootForce, maxShootForce, progress);
            curProjectile.parent = null;
            Rigidbody projRb = curProjectile.GetComponent<Rigidbody>();
            projRb.isKinematic = false;
            projRb.AddForce(projectileHolder.up* force, ForceMode.Impulse);
        }
    }

    [ContextMenu("Reload")]
    public void reloadProjectile()
    {
        if(curProjectile != null)
        {
            Destroy(curProjectile.gameObject);
        }

        curProjectile = Instantiate(projectilePref, projectileHolder).transform;
    }
    IEnumerator ShootCoroutine()
    {
        float timer = 0;

        float curDonwAngle = GetAngle(progress);
        float curShootTime = maxShootTimer * progress;

        hingeJS.spring = shootForce;
        while (timer < curShootTime)
        {
            hingeJS.targetPosition = (curDonwAngle - upAngle) * shootCurve.Evaluate(timer/ curShootTime) + upAngle;
            spoonHinge.spring = hingeJS;
            yield return null;
            timer += Time.deltaTime;
        }

        throwProjectile();
        hingeJS.spring = hingeJSSpring;
        progress = 0;
        shooting = false;
    }
}
