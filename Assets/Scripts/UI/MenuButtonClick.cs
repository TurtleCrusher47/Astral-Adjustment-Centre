using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuButtonClick : MonoBehaviour
{
    public UnityEvent unityEvent = new UnityEvent();
    public GameObject button;
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
                    button.GetComponentInChildren<TMP_Text>().color = new Color32(0, 0, 0, 255);
                }
                catch {}
            }
            else if (onButton && value == false)
            {
                // Not On Button
                try
                {
                    button.GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 255);
                }
                catch {}
            }

            _onButton = value;
        }
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
            Debug.Log(hit.transform.name);
            unityEvent.Invoke();
        }
    }
}
