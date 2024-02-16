using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponRigTarget : MonoBehaviour
{
    [Header("Rig Targets")]
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform leftHandTransform;

    [Header("Rig Hints")]
    [SerializeField] private Transform rightHandHint;
    [SerializeField] private Transform leftHandHint;


    [Header("Target Transforms")]
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform leftHandTarget;

    [SerializeField] private Transform rightElbowTarget;
    [SerializeField] private Transform leftElbowTarget;


    void Update()
    {
        rightHandTransform.position = rightHandTarget.position;
        leftHandTransform.position = leftHandTarget.position;

        rightHandHint.position = rightElbowTarget.position;
        leftHandHint.position = leftElbowTarget.position;

    }
}
