using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuButtonClick : MonoBehaviour
{
    public UnityEvent unityEvent = new UnityEvent();
    public GameObject button;
    public Color32 baseColor;
    private Ray ray;
    private RaycastHit hit;
    private bool _onButton = false;
    public bool onButton
    {
        get { return _onButton; }
        set
        {
            if (!_onButton && value == true)
            {
                // On Button
                try
                {
                    button.GetComponentInChildren<TMP_Text>().color = new Color32(128, 128, 128, 255);
                }
                catch {}
            }
            else if (onButton && value == false)
            {
                // Not On Button
                try
                {
                    button.GetComponentInChildren<TMP_Text>().color = baseColor;
                }
                catch {}
            }

            _onButton = value;
        }
    }

    void Awake()
    {
        try
        {
            baseColor = gameObject.GetComponentInChildren<TMP_Text>().color;
        }
        catch {}
    }

    void Start()
    {
        button = this.gameObject;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (!onButton && hit.collider.gameObject == gameObject)
            {
                onButton = true;
            }
            else if (onButton && hit.collider.gameObject != gameObject)
            {
                onButton = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && onButton)
        {
            unityEvent.Invoke();
        }
    }
}
