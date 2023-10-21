using System.Collections;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Gun Settings")]
    public float damage = 5f;
    public float range = 50f;
    [SerializeField] public float reloadTime;
    WaitForSeconds reloadWait;

    [Header("Aim Down Sights Settings")]
    [SerializeField] public bool aimDownSights = false;
    [SerializeField] public Transform defaultPos;
    [SerializeField] public Transform adsPos;

    [Header("Effect Settings")]
    [SerializeField] public GameObject hitEffect;
    [SerializeField] public GameObject muzzleFlash;
    [SerializeField] public Transform flashPoint;

    [Header("Ammo Settings")]
    [SerializeField] public int magSize;
    [SerializeField] public int maxAmmo;

    [Header("Automatic Weapon Settings")]
    public bool automatic = false;
    public float fireRate = 10f;
    WaitForSeconds autoWait;

    [Header("Sound Settings")]
    [SerializeField] public AudioSource fireSound;
    [SerializeField] public AudioSource noAmmoSound;
    [SerializeField] public AudioSource outOfAmmo;


    [Header("Recoil Settings")]
    public bool recoil = false;
    public float recoilIncrease = 0f;
    public float maxSpreadAngle = 0f;
    public Vector3 recoilKick = new Vector3(0f, 0f, -0.5f);
    public float recoilSpeed = 2f;

    int currentAmmo;
    float currentSpread = 0f;

    protected override void Start()
    {
        base.Start();

        autoWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = magSize;

        UpdateDisplay();
      
        if (defaultPos == null && adsPos == null)
        {
            defaultPos = transform;
            adsPos = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.shoot && !automatic && CanShoot())
        {
            Primary();
        } else if (_input.shoot && !automatic && !CanShoot())
        {
            noAmmoSound.Play();
        }

        _input.shoot = false;

        Secondary();
    }

    public override void Primary()
    {
        currentAmmo--;
        UpdateDisplay();

        if (currentAmmo > 0) 
        { 
            fireSound.Play();
            GameObject flash = Instantiate(muzzleFlash, flashPoint);
            Destroy(flash, 0.5f);
        } else
        {
            noAmmoSound.Play();
        }

        RaycastHit hit;

        Vector3 direction;

        if (recoil)
        {
            float spreadAngle = Random.Range(-currentSpread, currentSpread);

            Quaternion spreadRotation = Quaternion.Euler(0f, 0f, spreadAngle);
            
            direction = spreadRotation * transform.forward;

            currentSpread += recoilIncrease;

            currentSpread = Mathf.Min(currentSpread, maxSpreadAngle);

        } else
        {
            direction = cam.transform.forward;
        }

        if (Physics.Raycast(cam.transform.position, direction, out hit, range))
        {
            Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            if (hit.collider.GetComponent<Damageable>() != null)
            {
                hit.collider.GetComponent<Damageable>().TakeDamage(damage);
            }
        }

    }

    public override void Secondary()
    {
        if (_input.ads && aimDownSights)
        {
            transform.position = adsPos.position;
            cam.m_Lens.FieldOfView = 70f;
        } else
        {
            transform.position = defaultPos.position;
            cam.m_Lens.FieldOfView = 103f;
        }
    }

    public IEnumerator RapidFire()
    {
        if (CanShoot())
        {
            Primary();
            if (automatic)
            {
                while (CanShoot())
                {
                    yield return autoWait;
                    Primary();
                }
                noAmmoSound.Play();
            }
        } else
        {
            noAmmoSound.Play();
        }
    }

    bool CanShoot()
    {
        if (currentAmmo > 0) return true;
        return false;
    }

    public IEnumerator Reload()
    {
        if (currentAmmo == 0 && maxAmmo == 0 || currentAmmo > 0 && maxAmmo == 0)
        {
            outOfAmmo.Play();
            yield return null;
        }

        if (currentAmmo == magSize)
        {
            yield return null;
        }

        yield return reloadWait;

        if (currentAmmo < magSize && maxAmmo > magSize)
        {
            int bulletsToReload = magSize - currentAmmo;
            int bulletsAvailableForReload = Mathf.Min(bulletsToReload, maxAmmo);

            maxAmmo -= bulletsToReload;
            currentAmmo += bulletsAvailableForReload;
        } else if (currentAmmo < magSize && maxAmmo <= magSize)
        {
            int bulletsNeeded = magSize - currentAmmo;

            if (bulletsNeeded >= maxAmmo) { 
                currentAmmo += maxAmmo;
                maxAmmo = 0;
            } else
            {
                currentAmmo += bulletsNeeded;
                maxAmmo -= bulletsNeeded;
            }
        }

        UpdateDisplay();
    }

    public override void UpdateDisplay()
    {
        base.UpdateDisplay();

        if (currentAmmo == 0 && maxAmmo == 0)
        {
            weaponStats.color = Color.red;
            weaponStats.text = "NO AMMO";
            return;
        }

        weaponStats.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();

        int half = magSize / 2;

        if (currentAmmo <= half)
        {
            weaponStats.color = Color.red;
        } else
        {
            weaponStats.color = Color.white;
        }
    }
}
