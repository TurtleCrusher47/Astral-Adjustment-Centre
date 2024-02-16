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
    [SerializeField] private GameObject menuTarget, friendsTarget, leaderboardTarget, guildsTarget, nextTarget;
    [SerializeField] private GameObject menuPanel, friendsPanel, leaderboardPanel, guildsPanel, nextPanel;
    [SerializeField] private Material menuMat, friendsMat, leaderboardMat, guildsMat;

    void Awake()
    {
        SetAllButtonClick(true);
        SetAllPanels(false);

        menuMat = menuTarget.GetComponent<Renderer>().material;
        friendsMat = friendsTarget.GetComponent<Renderer>().material;
        leaderboardMat = leaderboardTarget.GetComponent<Renderer>().material;
        guildsMat = guildsTarget.GetComponent<Renderer>().material;
    }

    private void SetAllButtonClick(bool active)
    {
        friendsButton.enabled = active;
        leaderboardButton.enabled = active;
        guildsButton.enabled = active;
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

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        switch (currCamera.name)
        {
            case "FriendsCamera":
                StartCoroutine(DelayedShowPanel(friendsPanel, null, false));
                StartCoroutine(HologramDissolve(friendsMat, false));
                break;
            case "LeaderboardCamera":
                StartCoroutine(DelayedShowPanel(leaderboardPanel, null, false));
                StartCoroutine(HologramDissolve(leaderboardMat, false));
                break;
            case "GuildsCamera":
                StartCoroutine(DelayedShowPanel(guildsPanel, null, false));
                StartCoroutine(HologramDissolve(guildsMat, false));
                break;
        }

        currCamera.Priority--;
        currCamera.gameObject.SetActive(false);
        currCamera = target;
        currCamera.gameObject.SetActive(true);
        currCamera.Priority++;

        switch (target.name)
        {
            case "FriendsCamera":
                StartCoroutine(DelayedShowPanel(friendsPanel, friendsMat, true));
                break;
            case "LeaderboardCamera":
                StartCoroutine(DelayedShowPanel(leaderboardPanel, leaderboardMat, true));
                break;
            case "GuildsCamera":
                StartCoroutine(DelayedShowPanel(guildsPanel, guildsMat, true));
                break;
        }
    }

    private IEnumerator TransitionToSocials()
    {
        currCamera = mainCamBrain.ActiveVirtualCamera as CinemachineVirtualCamera;

        yield return new WaitUntil(() => currCamera != null);

        StartCoroutine(ChangeCameraPriority(creditsCamera, null, false));
        StartCoroutine(DelayedShowPanel(menuPanel, null, false));
        StartCoroutine(HologramDissolve(menuMat, false));

        yield return new WaitUntil(() => mainCamBrain.IsBlending);
        yield return new WaitForSeconds(1.25f);

        StartCoroutine(ChangeCameraPriority(secondFloorEntranceCamera, null, false));

        yield return new WaitUntil(() => mainCamBrain.IsBlending);
        yield return new WaitUntil(() => !mainCamBrain.IsBlending);

        StartCoroutine(ChangeCameraPriority(socialsCamera, null, false));
    }

    private IEnumerator HologramDissolve(Material mat, bool show)
    {
        float dissolveValue = mat.GetFloat("_DissolveValue");

        if (show)
        {
            while (dissolveValue > 0)
            {
                dissolveValue -= Time.deltaTime;
                mat.SetFloat("_DissolveValue", dissolveValue);

                yield return null;
            }
        }
        else
        {
            while (dissolveValue < 1)
            {
                dissolveValue += Time.deltaTime;
                mat.SetFloat("_DissolveValue", dissolveValue);

                yield return null;
            }
        }
    }

    private IEnumerator DelayedShowPanel(GameObject panel, Material mat, bool show)
    {
        if (show)
        {
            yield return new WaitUntil(() => mainCamBrain.IsBlending);

            yield return new WaitForSeconds(0.25f);

            StartCoroutine(HologramDissolve(mat, show));

            yield return new WaitUntil(() => !mainCamBrain.IsBlending);
        }

        panel.SetActive(show);
    }

    private IEnumerator ChangeCameraPriority(CinemachineVirtualCamera target, Material mat, bool inSocials)
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
            StartCoroutine(DelayedShowPanel(nextPanel, mat, true));
        }

        yield return null;
    }
}
