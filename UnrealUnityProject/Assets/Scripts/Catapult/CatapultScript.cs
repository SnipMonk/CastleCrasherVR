using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultScript : MonoBehaviour
{

    [SerializeField] Transform spoonTrans;
    [SerializeField] Transform projectileHolder;
    [SerializeField] GameObject projectile;

    [Range(0f, 1f)]
    [SerializeField] float force;

    GameObject curProjectile;
    [SerializeField] float lowRot;
    [SerializeField] float hihgRot;


    [SerializeField] AnimationCurve shootSpoonCurveMin;
    [SerializeField] AnimationCurve shootSpoonCurveMax;
    [SerializeField] float shootTime;

    [SerializeField] float shootForceMin;
    [SerializeField] float shootForceMax;

    [SerializeField] float recoiTime;

    [SerializeField] AnimationCurve recoilCuveMin;
    [SerializeField] AnimationCurve recoilCuveMax;

    Vector3 startRot;
    private bool isShooting = false;
    IEnumerator ShootCoroutine()
    {

        float timer = 0;

        Vector3 rot = spoonTrans.rotation.eulerAngles;
        print(rot);

        while (timer < shootTime)
        {
            float angle = Mathf.Lerp(shootSpoonCurveMin.Evaluate(timer / shootTime), shootSpoonCurveMax.Evaluate(timer / shootTime), force);
            rot.x = Mathf.Lerp(lowRot, hihgRot, angle);
            spoonTrans.rotation = Quaternion.Euler(rot);

            yield return null;
            timer += Time.deltaTime;
        }

        Rigidbody rb = curProjectile.GetComponent<Rigidbody>();

        float shootForce = Mathf.Lerp(shootForceMin, shootForceMax, force);

        
        rb.isKinematic = false;
        rb.AddForce(projectileHolder.up * shootForce, ForceMode.Impulse);
        curProjectile.transform.parent = null;

        print(rot);
        StartCoroutine(RecoilCoroutine(rot));
        yield break;
    }

    IEnumerator RecoilCoroutine(Vector3 rot)
    {
        float timer = 0;

        print(rot);

        while (timer < recoiTime)
        {
            float angle = Mathf.Lerp(recoilCuveMin.Evaluate(timer / recoiTime), recoilCuveMax.Evaluate(timer / recoiTime), force);
            rot.x = Mathf.Lerp(lowRot, hihgRot, angle);
            spoonTrans.rotation = Quaternion.Euler(rot);

            yield return null;
            timer += Time.deltaTime;
        }
        print("Endedn");
        yield break;
    }
    private void Shoot()
    {
        print("Shooted");
        isShooting = true;
        StartCoroutine(ShootCoroutine());
    }

    private void Start()
    {
        curProjectile = Instantiate(projectile, projectileHolder);
        startRot = spoonTrans.rotation.eulerAngles;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && !isShooting)
        {
            Shoot();
        }
        if (Input.GetButton("Fire2"))
        {
            Reload();
        }
    }

    private void Reload()
    {
        Destroy(curProjectile);
        curProjectile = Instantiate(projectile, projectileHolder);
        isShooting = false;
        spoonTrans.rotation = Quaternion.Euler(startRot);
    }
}
