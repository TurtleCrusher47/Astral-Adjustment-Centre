using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    [SerializeField] protected RangedWeaponData rangedWeaponData;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected LayerMask targetLayers;
    protected Transform cam;
    protected Transform orientation;
    protected Recoil recoil;
    protected Transform camRotation;
    // [SerializeField] protected Recoil recoil;

    // [SerializeField] protected UpdateAmmoText updateAmmoText;

    protected float timeSinceLastShot;

    protected override void Start()
    {
        cam = GameObject.FindGameObjectWithTag("CameraHolder").transform;
        orientation = GameObject.FindGameObjectWithTag("Orientation").transform;
        camRotation = GameObject.FindGameObjectWithTag("CameraRotation").transform;
        recoil = camRotation.GetComponent<Recoil>();
    }

    public void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        // Left mouse button is being held down
        if (Input.GetMouseButton(0))
        {
            UsePrimary();
        }
        if (Input.GetKeyDown(KeyCode.R) && !rangedWeaponData.infiniteAmmo)
        {
            StartReload();
        }
        // Right mouse button is clicked
        if (Input.GetMouseButton(1))
        {
            UseSecondary();
        }
        if (Input.GetMouseButtonUp(1))
        {
            UseSecondaryUp();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseAbility();
        }

        // Cooldowns
        if (secondaryCooldownTimer > 0)
        {
            secondaryCooldownTimer -= Time.deltaTime;
        }

        if (abilityCooldownTimer > 0)
        {
            abilityCooldownTimer -= Time.deltaTime;
        }
    }

    protected bool CanShoot()
    {
        return !rangedWeaponData.reloading && timeSinceLastShot > 1f / (rangedWeaponData.fireRate / 60f);
    }

    // Probably change later as we will not do disabling
    private void OnDisable()
    {
        rangedWeaponData.reloading = false;
    }

    public void StartReload()
    {
        if (!rangedWeaponData.reloading && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reloading");

        rangedWeaponData.reloading = true;

        yield return new WaitForSeconds(rangedWeaponData.reloadTime);

        rangedWeaponData.currentAmmo = rangedWeaponData.magazineSize;

        rangedWeaponData.reloading = false;

        // updateAmmoText.UpdateAmmo(gunData.currentAmmo, gunData.magazineSize);
    }

    public int GetAmmo()
    {
        return rangedWeaponData.currentAmmo;
    }
}
