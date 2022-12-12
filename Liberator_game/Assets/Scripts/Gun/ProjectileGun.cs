using UnityEngine;
using TMPro;
public class ProjectileGun : MonoBehaviour
{
    //bullet
    public GameObject bullet;
    //bullet force
    public float shootForce, upwardForce;
    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private float maxLifetime = 3f;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    //camera and transform
    public Camera fpsCam;
    public Transform attackPoint;
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunition;
    public bool allowInvoke = true;

    private void Awake()
    {
        //make sure mag is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update() 
    {
        MyInput();
        //count down lifetime
        maxLifetime -= Time.deltaTime;
        if(maxLifetime <= 0)
            Destroy(bullet);
        //set ammo hud
        if(ammunition != null)
            ammunition.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    private void MyInput()
    {
        // check if allowed to shoot
        if(allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //reloading
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //reload automatically with an empty clip
        if(readyToShoot && shooting && !reloading && bulletsLeft <=0) Reload();

        //shoot
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //bulletShot = 0
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Aim at crosshair
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);
        
        //calculate direction
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //create bullet obj
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //rotate bullet
        currentBullet.transform.forward = directionWithSpread.normalized;
        //add phsyics to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);

        //create muzzleFlash
        if(muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;
        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
