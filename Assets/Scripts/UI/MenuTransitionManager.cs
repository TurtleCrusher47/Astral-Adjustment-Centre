using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuTransitionManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> virtualCameras;
    [SerializeField] private CinemachineVirtualCamera currCamera;
    [SerializeField] private CinemachineBrain mainCamBrain;
    [SerializeField] private CinemachineVirtualCamera menuCamera;
    [SerializeField] private CinemachineVirtualCamera loadSceneCamera;
    [SerializeField] private GameObject loginPanel, settingsPanel, creditsPanel;

    void Awake()
    {
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            virtualCameras[i].SetActive(false);
        }

        virtualCameras[0].SetActive(true);

        loginPanel.SetActive(true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        currCamera.Priority++;
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        StopAllCoroutines();

        switch (currCamera.name)
        {
            case "StartCamera":
                StartCoroutine(DelayedShowPanel(loginPanel, false));
                break;
            case "SettingsCamera":
                StartCoroutine(DelayedShowPanel(settingsPanel, false));
                break;
            case "CreditsCamera":
                StartCoroutine(DelayedShowPanel(creditsPanel, false));
                break;
        }

        currCamera.Priority--;
        currCamera.gameObject.SetActive(false);
        currCamera = target;
        currCamera.gameObject.SetActive(true);
        currCamera.Priority++;

        switch (target.name)
        {
            case "SettingsCamera":
                StartCoroutine(DelayedShowPanel(settingsPanel, true));
                break;
            case "CreditsCamera":
                StartCoroutine(DelayedShowPanel(creditsPanel, true));
                break;
            case "PlayCamera":
                StartCoroutine(DelayedLoadScene());
                break;
        }
    }

    private IEnumerator DelayedShowPanel(GameObject panel, bool show)
    {
        if (show)
        {
            yield return new WaitUntil(() => mainCamBrain.IsBlending);

            yield return new WaitUntil(() => !mainCamBrain.IsBlending);
        }

        panel.SetActive(show);
    }

    private IEnumerator DelayedLoadScene()
    {
        yield return new WaitUntil(() => mainCamBrain.IsBlending);

        yield return new WaitUntil(() => !mainCamBrain.IsBlending);

        yield return new WaitForSeconds(0.15f);

        currCamera.Priority--;
        currCamera = loadSceneCamera;
        currCamera.Priority++;

        yield return new WaitForSeconds(0.75f);

        GameManager.Instance.ChangeScene("LevelScene");
    }
}
