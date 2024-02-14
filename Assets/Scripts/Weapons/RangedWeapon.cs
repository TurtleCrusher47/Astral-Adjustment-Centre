using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    [SerializeField] protected RangedWeaponData rangedWeaponData;
    [SerializeField] protected Transform cam;
    [SerializeField] protected Transform firePoint;
    // [SerializeField] protected Recoil recoil;

    // [SerializeField] protected UpdateAmmoText updateAmmoText;

    protected float timeSinceLastShot;

    protected override void Init()
    {
    }

    public void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && !rangedWeaponData.infiniteAmmo)
        {
            StartReload();
        }
    }
    

    protected bool CanShoot()
    {
        return !rangedWeaponData.reloading && timeSinceLastShot > 1f / (rangedWeaponData.fireRate / 60f);
    }

    // Dependent on gun as Raycast Guns have the same shoot code but GameObject guns do not
    public abstract void Shoot();

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

    // Effects
    public abstract void OnShot();

    public int GetAmmo()
    {
        return rangedWeaponData.currentAmmo;
    }
}
