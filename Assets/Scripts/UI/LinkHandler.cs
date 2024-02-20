using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class LinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textBox;
    private Canvas canvasToCheck;
    private Camera cameraToUse;

    public delegate void ClickOnLinkEvent(string keyword);
    public static event ClickOnLinkEvent OnClickedLinkEvent;

    void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        canvasToCheck = GetComponentInParent<Canvas>();

        if (canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            cameraToUse = null;
        }
        else
        {
            cameraToUse = canvasToCheck.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);

        var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(textBox, mousePosition, cameraToUse);

        if (linkTaggedText == -1)
        {
            return;
        }

        TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[linkTaggedText];

        string linkID = linkInfo.GetLinkID();
        
        if (linkID.Contains("https"))
        {
            Application.OpenURL(linkID);
            return;
        }

        OnClickedLinkEvent?.Invoke(linkInfo.GetLinkText());
    }
}
