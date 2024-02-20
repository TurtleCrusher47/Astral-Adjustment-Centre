using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackIndicator : MonoBehaviour
{
    [SerializeField] private GameObject maxDistance;
    [SerializeField] private GameObject chargeDistance;
    private Vector3 max;
    private float charge;

    private void Update()
    {
        // updates max distance
        maxDistance.transform.localScale = new Vector3(max.x, 0.25f, max.z);
        // calculates & updates curr charging distance
        charge = Mathf.Max(Mathf.Min(charge, max.z), 0);
        chargeDistance.transform.localScale = new Vector3(max.x, 0.26f, charge);
        chargeDistance.transform.localPosition = new Vector3(0, 0, (0.5f * max.z) - (charge / 2));
    }

    public void DoCharge(Vector3 pos, Vector3 rotation, Vector3 maxScale, float chargeTime, float currTime)
    {
        // set pos and scale
        transform.position = pos;
        transform.eulerAngles = rotation;
        max = maxScale;
        // set charge level
        charge = (chargeTime - currTime) * (max.z / chargeTime);
    }
}
