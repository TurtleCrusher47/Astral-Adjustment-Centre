using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackIndicator : MonoBehaviour
{
    private enum AttackType
    {
        NULL,
        QUAD,
        CIRCLE
    }

    [SerializeField] private GameObject maxDistanceQ;
    [SerializeField] private GameObject chargeDistanceQ;
    [SerializeField] private GameObject maxDistanceC;
    [SerializeField] private GameObject chargeDistanceC;
    private PlayerCombat player;
    private Vector3 max;
    private float charge;
    private AttackType type;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        type = AttackType.NULL;
    }

    private void Update()
    {
        if (type != AttackType.NULL)
        {
            UpdateIndicators();
        }
    }

    private void UpdateIndicators()
    {
        switch (type)
        {
            case AttackType.QUAD:
                maxDistanceQ.SetActive(true);
                chargeDistanceQ.SetActive(true);
                maxDistanceC.SetActive(false);
                chargeDistanceC.SetActive(false);
                // updates max distance
                maxDistanceQ.transform.localScale = new Vector3(max.x, 0.5f, max.z);
                // calculates & updates curr charging distance
                charge = Mathf.Max(Mathf.Min(charge, max.z), 0);
                chargeDistanceQ.transform.localScale = new Vector3(max.x, 0.5f, charge);
                chargeDistanceQ.transform.localPosition = new Vector3(0, 0, -(0.5f * max.z) + (charge / 2));

                break;

            case AttackType.CIRCLE:
                maxDistanceC.SetActive(true);
                chargeDistanceC.SetActive(true);
                maxDistanceQ.SetActive(false);
                chargeDistanceQ.SetActive(false);
                // updates max distance
                maxDistanceC.transform.localScale = new Vector3(max.x, 0.25f, max.z);
                // calculates & updates curr charging distance
                charge = Mathf.Max(Mathf.Min(charge, max.z), 0);
                chargeDistanceC.transform.localScale = new Vector3(charge, 0.25f, charge);

                break;
        }
    }

    public void SetChargeParameters(Vector3 pos, Vector3 offset, float angle, Vector3 maxScale)
    {
        // set pos and scale
        transform.eulerAngles = new Vector3(0, angle, 0);
        transform.position = new Vector3(pos.x + offset.x, transform.position.y, pos.z + offset.z);
        max = maxScale;
        type = AttackType.QUAD;
    }

    public void SetChargeParameters(Vector3 pos, Vector3 maxScale)
    {
        // set pos and scale
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        max = maxScale;
        type = AttackType.CIRCLE;
    }

    public void DoCharge(float chargeTime, float currTime)
    {
        // set charge level
        charge = (chargeTime - currTime) * (max.z / chargeTime);
    }

    public void ActivateHit(float dmg)
    {
        Collider[] colliders;
        switch (type)
        {
            case AttackType.QUAD:
                colliders = Physics.OverlapBox(transform.position, (maxDistanceQ.transform.localScale / 2) + new Vector3(0, maxDistanceQ.transform.localScale.y, 0));
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("PlayerCollider"))
                    {
                        //player.Damage(dmg);
                        Debug.Log("Player Hit");
                    }
                }

                break;

            case AttackType.CIRCLE:
                colliders = Physics.OverlapSphere(transform.position, maxDistanceC.transform.localScale.x / 2);
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("PlayerCollider"))
                    {
                        //player.Damage(dmg);
                        Debug.Log("Player Hit");
                    }
                }

                break;
        }
    }
}
