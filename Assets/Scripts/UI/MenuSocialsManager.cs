using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MenuSocialsManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain mainCamBrain;
    [SerializeField] private CinemachineVirtualCamera currCamera;
    [SerializeField] private CinemachineVirtualCamera creditsCamera, secondFloorEntranceCamera;
    [SerializeField ] private CinemachineVirtualCamera socialsCamera;
    [SerializeField] private CinemachineVirtualCamera friendsCamera;
    [SerializeField] private CinemachineVirtualCamera leaderboardCamera;
    [SerializeField] private CinemachineVirtualCamera guildsCamera;

    [SerializeField] private MenuButtonClick friendsButton, leaderboardButton, guildsButton;
    [SerializeField] private GameObject friendsTarget, leaderboardTarget, guildsTarget, nextTarget;
    [SerializeField] private GameObject friendsPanel, leaderboardPanel, guildsPanel, nextPanel;

    void Awake()
    {
        SetAllButtonClick(true);
        SetAllTargets(false);
        SetAllPanels(false);
    }

    private void SetAllButtonClick(bool active)
    {
        friendsButton.enabled = active;
        leaderboardButton.enabled = active;
        guildsButton.enabled = active;
    }

    private void SetAllTargets(bool active)
    {
        friendsTarget.SetActive(active);
        leaderboardTarget.SetActive(active);
        guildsTarget.SetActive(active);
    }

    private void SetAllPanels(bool active)
    {
        friendsPanel.SetActive(active);
        leaderboardPanel.SetActive(active);
        guildsPanel.SetActive(active);
    }

    public void GoToSocials()
    {
        Debug.Log("GoToSocials()");
        StopAllCoroutines();

        StartCoroutine(TransitionToSocials());
    }

    public void SwitchPanel(CinemachineVirtualCamera target)
    {
        bool inSocials = false;

        switch (target.name)
        {
            case "FriendsCamera":
                SetAllButtonClick(false);
                nextTarget = friendsTarget;
                nextPanel = friendsPanel;
                inSocials = true;
                break;
            case "LeaderboardCamera":
                SetAllButtonClick(false);
                nextTarget = leaderboardTarget;
                nextPanel = leaderboardPanel;
                inSocials = true;
                break;
            case "GuildsCamera":
                SetAllButtonClick(false);
                nextTarget = guildsTarget;
                nextPanel = guildsPanel;
                inSocials = true;
                break;
            case "SocialsCamera":
                SetAllButtonClick(true);
                break;
        }

        SetAllTargets(false);
        SetAllPanels(false);

        StartCoroutine(ChangeCameraPriority(target, inSocials));
    }

    private IEnumerator TransitionToSocials()
    {
        currCamera = mainCamBrain.ActiveVirtualCamera as CinemachineVirtualCamera;

        yield return new WaitUntil(() => currCamera != null);

        StartCoroutine(ChangeCameraPriority(creditsCamera, false));

        yield return new WaitUntil(() => mainCamBrain.IsBlending);
        yield return new WaitForSeconds(1.25f);

        StartCoroutine(ChangeCameraPriority(secondFloorEntranceCamera, false));

        yield return new WaitUntil(() => mainCamBrain.IsBlending);
        yield return new WaitUntil(() => !mainCamBrain.IsBlending);

        StartCoroutine(ChangeCameraPriority(socialsCamera, false));
    }

    private IEnumerator ChangeCameraPriority(CinemachineVirtualCamera target, bool inSocials)
    {
        if (inSocials)
        {
            nextTarget.SetActive(true);
        }

        currCamera.Priority--;
        currCamera.gameObject.SetActive(false);
        currCamera = target;
        currCamera.gameObject.SetActive(true);
        currCamera.Priority++;

        if (inSocials)
        {
            yield return new WaitUntil(() => mainCamBrain.IsBlending);
            yield return new WaitUntil(() => !mainCamBrain.IsBlending);

            nextPanel.SetActive(true);
        }

        yield return null;
    }
}
