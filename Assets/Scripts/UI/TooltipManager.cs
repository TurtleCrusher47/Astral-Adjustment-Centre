using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    [SerializeField] TMP_Text tooltipText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetAndShowToolTip(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        tooltipText.text = string.Empty;
    }
}
