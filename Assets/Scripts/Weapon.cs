using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject shooter;

    public GameObject explosionEffect;
    public LineRenderer lineRenderer;

    private Transform _firePoint;

    private void Awake()
    {
        _firePoint = transform.Find("FirePoint");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Shoot();
        //Invoke("Shoot", 1f);
        //Invoke("Shoot", 2f);
        //Invoke("Shoot", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if(bulletPrefab != null && _firePoint != null && shooter != null)
        {
            GameObject myBullet = Instantiate(bulletPrefab, _firePoint.position, Quaternion.identity) as GameObject;

            Bullet bulletComponent = myBullet.GetComponent<Bullet>();

            if (shooter.transform.localScale.x < 0f)
            {
                //Left
                bulletComponent.direction = Vector2.left; // new Vector2(-1f, 0f)
            }
            else
            {
                //Right
                bulletComponent.direction = Vector2.right; // new Vector2(1f, 0f)
            }
        }
    }

    public IEnumerator ShootWithRaycast()
    {
        if(explosionEffect != null && lineRenderer != null)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(_firePoint.position, _firePoint.right);

            if (hitInfo)
            {
                // Example code
                // if(hitInfo.collider.tag == "Player")
                //{
                //    Transform player = hitInfo.transform;
                //    player.GetComponent<PlayerHealth>().ApllyDamahe(5);
                //}

                // Instantiate explosion on hit point
                Instantiate(explosionEffect, hitInfo.point, Quaternion.identity);

                // Set Line renderer
                lineRenderer.SetPosition(0, _firePoint.position);
                lineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                lineRenderer.SetPosition(0, _firePoint.position);
                lineRenderer.SetPosition(1, hitInfo.point + Vector2.right * 100);
            }
        

            lineRenderer.enabled = true;

            yield return null;

            lineRenderer.enabled = false;
        }
    }
}
