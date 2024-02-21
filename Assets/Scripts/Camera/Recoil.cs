using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;
    
    // Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;
   
    void Update()
    {
        targetRotation  = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void GunRecoil(Vector3 recoil)
    {
        targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
    }
}
