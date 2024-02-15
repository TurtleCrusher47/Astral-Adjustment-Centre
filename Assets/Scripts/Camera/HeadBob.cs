using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;

    [Range(0.001f, 0.01f)]
    public float amount = 0.002f;

    [Range(1f, 30f)]
    public float frequency = 10.0f;

    [Range(10f, 100f)]
    public float smooth = 10f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
       CheckBobTrigger();
       StopHeadBob();
    }

    private void CheckBobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            BeginHeadBob();
        }
    }

    private Vector3 BeginHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, smooth * Time.deltaTime);
        transform.localPosition += pos;

        weaponHolder.localPosition -= pos;

        return pos;
    }

    private void StopHeadBob()
    {
        if (transform.localPosition == startPos)
        return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 1 * Time.deltaTime);
    }
}
