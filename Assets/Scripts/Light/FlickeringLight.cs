using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private VisualEffect _visualEffect;
    [SerializeField] private Light _light;
    public float multiplier;
    public float fadeSpeed;

    private bool started = false;

    void Awake()
    {
        _visualEffect = GetComponentInParent<VisualEffect>();
        _light = GetComponent<Light>();
    }

    void OnEnable()
    {
        started = false;
    }

    void Update()
    {
        _light.intensity = Random.Range(1, 3) * multiplier;
        multiplier -= fadeSpeed * Time.deltaTime;

        if (!started)
        {
            if (_visualEffect.aliveParticleCount > 0)
            {
                started = true;
            }
        }
        else if (_visualEffect.aliveParticleCount <= 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
