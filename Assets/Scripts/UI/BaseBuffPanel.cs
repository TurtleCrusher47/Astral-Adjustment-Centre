using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseBuffPanel : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descText;

    public void UpdateBuffInfo(string title, string desc)
    {
        titleText.text = title;
        descText.text = desc;
    }
}
