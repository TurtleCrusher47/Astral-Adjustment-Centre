using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordToggle : MonoBehaviour
{
    [SerializeField] private Image bgImage;

    void Start()
    {
        bgImage.enabled = true;
    }

    public void ToggleSprite()
    {
        if (bgImage.enabled)
        {
            bgImage.enabled = false;
        }
        else
        {
            bgImage.enabled = true;
        }
    }
}
