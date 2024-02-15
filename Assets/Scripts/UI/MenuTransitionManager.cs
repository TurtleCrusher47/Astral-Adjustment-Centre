using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitionManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> virtualCameras;
    [SerializeField] private CinemachineVirtualCamera currCamera;
    [SerializeField] private CinemachineBrain mainCamBrain;
    [SerializeField] private CinemachineVirtualCamera menuCamera;
    [SerializeField] private GameObject loginPanel, settingsPanel;

    void Awake()
    {
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            virtualCameras[i].SetActive(true);
        }

        loginPanel.SetActive(true);
        settingsPanel.SetActive(false);

        currCamera.Priority++;
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        StopAllCoroutines();

        if (currCamera.name == "StartCamera")
        {
            loginPanel.SetActive(false);
        }
        if (currCamera.name == "SettingsCamera")
        {
            StartCoroutine(DelayedSettingsPanel(false));
        }

        currCamera.Priority--;
        currCamera = target;
        currCamera.Priority++;

        if (target.name == "SettingsCamera")
        {
            StartCoroutine(DelayedSettingsPanel(true));
        }

        else if (target.name == "StartCamera")
        {
            StartCoroutine(DelayedLoadScene());
        }
    }

    private IEnumerator DelayedTransition(CinemachineVirtualCamera target, float delay)
    {
        yield return new WaitForSeconds(delay);

        UpdateCamera(target);
    }

    private IEnumerator DelayedSettingsPanel(bool show)
    {
        if (show)
        {
            yield return new WaitUntil(() => mainCamBrain.IsBlending);

            yield return new WaitUntil(() => !mainCamBrain.IsBlending);
        }

        settingsPanel.SetActive(show);
    }

    private IEnumerator DelayedLoadScene()
    {
        yield return new WaitUntil(() => mainCamBrain.IsBlending);

        yield return new WaitUntil(() => !mainCamBrain.IsBlending);

        yield return new WaitForSeconds(1);

        PlayFabManager.ChangeScene("LevelScene");
    }
}
