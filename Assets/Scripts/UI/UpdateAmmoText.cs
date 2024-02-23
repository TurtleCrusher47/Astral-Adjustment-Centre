using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateAmmoText : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
       ammoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
    }

    public void ClearText()
    {
        ammoText.text = "";
    }
}
