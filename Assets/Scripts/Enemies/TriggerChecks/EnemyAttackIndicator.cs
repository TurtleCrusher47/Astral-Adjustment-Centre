using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackIndicator : MonoBehaviour
{
    [SerializeField] private GameObject maxDistance;
    [SerializeField] private GameObject chargeDistance;
    private PlayerCombat player;
    private Vector3 max;
    private float charge;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        // updates max distance
        maxDistance.transform.localScale = new Vector3(max.x, 0.5f, max.z);
        // calculates & updates curr charging distance
        charge = Mathf.Max(Mathf.Min(charge, max.z), 0);
        chargeDistance.transform.localScale = new Vector3(max.x, 0.5f, charge);
        chargeDistance.transform.localPosition = new Vector3(0, 0, -(0.5f * max.z) + (charge / 2));
    }

    public void SetChargeParameters(Vector3 pos, Vector3 rotation, Vector3 maxScale)
    {
        // set pos and scale
        transform.eulerAngles = rotation;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        max = maxScale;
    }

    public void DoCharge(float chargeTime, float currTime)
    {
        // set charge level
        charge = (chargeTime - currTime) * (max.z / chargeTime);
    }

    public void ActivateHit(float dmg)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, maxDistance.transform.localScale / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("PlayerCollider"))
            {
                player.Damage(dmg);
                Debug.Log("Player Hit");
            }
        }
    }
}
