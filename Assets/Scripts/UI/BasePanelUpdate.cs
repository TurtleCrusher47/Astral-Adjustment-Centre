using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanelUpdate : MonoBehaviour
{
    public BaseBuffPanel baseBuffController;

    public void UpdateBuffInfo(string title, string description)
    {
            baseBuffController.UpdateBuffInfo(title, description);
    }
}
