using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public string text;

    private void OnMouseEnter()
    {
        TooltipManager.instance.SetAndShowToolTip(text);
    }

    private void OnMouseExit()
    {
        TooltipManager.instance.HideToolTip();
    }
}
