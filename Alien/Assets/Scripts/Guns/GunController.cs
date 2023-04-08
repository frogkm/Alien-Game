using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private KeyCode shootKey;
    [SerializeField] private KeyCode reloadKey;
    [SerializeField] private Transform bulletSpawnTrans;
    [SerializeField] private TMP_Text bulletText;

    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int magSize = 8;
    [SerializeField] private int bulletsPerTap = 1;
    [SerializeField] private float timeBetweenShots = 0.2f;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private bool isAutomatic = false;
    [SerializeField] private float spreadAmount = 0f;
    [SerializeField] private int numBulletsInSpread = 1;

    private int bulletsLeft;
    private bool canShoot;
    private bool reloading;


    void Start() {
        bulletsLeft = magSize;
        canShoot = true;
        reloading = false;
        updateUI();
    }

    void Update() {
        bool shootInput = isAutomatic ? Input.GetKey(shootKey) : Input.GetKeyDown(shootKey);  


        if(shootInput && canShoot) {
            if (!reloading && bulletsLeft == 0) {
                BeginReload();
            }
            else if(!reloading) {
                canShoot = false;
                Shoot();
                Invoke("ResetCanShoot", timeBetweenShots);
            }
        }

        if (Input.GetKey(reloadKey)) {
            if (!reloading && bulletsLeft != magSize) {
                BeginReload();
            }
        }
        updateUI();
    }

    private void Reload() {
        bulletsLeft = magSize;
        reloading = false;
    }

    private void BeginReload() {
        reloading = true;
        Invoke("Reload", reloadTime);
    }

    private void updateUI() {
        bulletText.text = (reloading ? "reloading    " : "") + string.Format("{0}/{1}", bulletsLeft, magSize);
    }

    private void ResetCanShoot() {
        canShoot = true;
    }

    private void Shoot()
    {
        bulletsLeft--;
        // Create a ray from the camera going through the middle of your screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit ;
        // Check whether your are pointing to something so as to adjust the direction
        Vector3 targetPoint ;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75) ; // You may need to change this value according to your needs
        
        Vector3 shotDirection = (targetPoint - bulletSpawnTrans.position).normalized;

        for (int i = 0; i < numBulletsInSpread; i++) {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTrans.position, Quaternion.identity);
            bullet.transform.rotation = bulletSpawnTrans.rotation;

            float xSpread = Random.Range(-spreadAmount, spreadAmount);
            float ySpread = Random.Range(-spreadAmount, spreadAmount);

            Vector3 finalShotDirection = (shotDirection + (new Vector3(xSpread, ySpread, 0))).normalized;

            bullet.transform.GetComponent<Rigidbody>().velocity = bulletSpeed * finalShotDirection;
        }
        

    }
    
}