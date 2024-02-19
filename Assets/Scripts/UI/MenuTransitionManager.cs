using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuTransitionManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutsceneDirector;
    [SerializeField] private GameObject subtitlePanel;
    [SerializeField] private List<GameObject> virtualCameras;
    [SerializeField] private CinemachineVirtualCamera currCamera;
    [SerializeField] private CinemachineBrain mainCamBrain;
    [SerializeField] private CinemachineVirtualCamera menuCamera;
    [SerializeField] private CinemachineVirtualCamera loadSceneCamera;
    [SerializeField] private GameObject loginPanel, menuPanel, settingsPanel, creditsPanel;
    [SerializeField] private GameObject loginTarget, menuTarget, settingsTarget, creditsTarget;
    [SerializeField] private Material loginMat, menuMat, settingsMat, creditsMat;

    void Awake()
    {
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            virtualCameras[i].SetActive(false);
        }

        virtualCameras[0].SetActive(true);

        loginPanel.SetActive(false);
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        loginMat = loginTarget.GetComponent<Renderer>().material;
        menuMat = menuTarget.GetComponent<Renderer>().material;
        settingsMat = settingsTarget.GetComponent<Renderer>().material;
        creditsMat = creditsTarget.GetComponent<Renderer>().material;

        currCamera.Priority++;

        StartCoroutine(DelayedShowLogin());
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        switch (currCamera.name)
        {
            case "StartCamera":
                StartCoroutine(DelayedShowPanel(loginPanel, null, false));
                StartCoroutine(HologramDissolve(loginMat, loginTarget, false));
                break;
            case "MenuCamera":
                StartCoroutine(DelayedShowPanel(menuPanel, null, false));
                StartCoroutine(HologramDissolve(menuMat, menuTarget, false));
                break;
            case "SettingsCamera":
                StartCoroutine(DelayedShowPanel(settingsPanel, null, false));
                StartCoroutine(HologramDissolve(settingsMat, settingsTarget, false));
                break;
            case "CreditsCamera":
                StartCoroutine(DelayedShowPanel(creditsPanel, null, false));
                StartCoroutine(HologramDissolve(creditsMat, creditsTarget, false));
                break;
        }

        currCamera.Priority--;
        currCamera.gameObject.SetActive(false);
        currCamera = target;
        currCamera.gameObject.SetActive(true);
        currCamera.Priority++;

        switch (target.name)
        {
            case "StartCamera":
                loginTarget.SetActive(true);
                StartCoroutine(DelayedShowPanel(loginPanel, loginMat, true));
                break;
            case "MenuCamera":
                menuTarget.SetActive(true);
                StartCoroutine(DelayedShowPanel(menuPanel, menuMat, true));
                break;
            case "SettingsCamera":
                settingsTarget.SetActive(true);
                StartCoroutine(DelayedShowPanel(settingsPanel, settingsMat, true));
                break;
            case "CreditsCamera":
                creditsTarget.SetActive(true);
                StartCoroutine(DelayedShowPanel(creditsPanel, creditsMat, true));
                break;
            case "PlayCamera":
                StartCoroutine(DelayedLoadScene());
                break;
        }
    }

    private IEnumerator DelayedShowLogin()
    {
        yield return new WaitForSeconds(0.5f);

        loginTarget.SetActive(true);
        StartCoroutine(HologramDissolve(loginMat, loginTarget, true));

        yield return new WaitForSeconds(0.75f);

        loginPanel.SetActive(true);
    }

    private IEnumerator DelayedShowPanel(GameObject panel, Material mat, bool show)
    {
        if (show)
        {
            yield return new WaitUntil(() => mainCamBrain.IsBlending);

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(HologramDissolve(mat, null, show));

            yield return new WaitUntil(() => !mainCamBrain.IsBlending);
        }

        panel.SetActive(show);
    }

    private IEnumerator HologramDissolve(Material mat, GameObject target, bool show)
    {
        float dissolveValue = mat.GetFloat("_DissolveValue");

        if (show)
        {
            AudioManager.Instance.PlaySFX("SFXHologramOpen");
            while (dissolveValue > 0)
            {
                dissolveValue -= Time.deltaTime;
                mat.SetFloat("_DissolveValue", dissolveValue);

                yield return null;
            }
        }
        else
        {
            AudioManager.Instance.PlaySFX("SFXHologramClose");
            while (dissolveValue < 1)
            {
                dissolveValue += Time.deltaTime;
                mat.SetFloat("_DissolveValue", dissolveValue);

                yield return null;
            }

            if (target != null)
            {
                target.SetActive(false);
            }
        }
    }

    private IEnumerator DelayedLoadScene()
    {
        yield return new WaitUntil(() => mainCamBrain.IsBlending);

        yield return new WaitUntil(() => !mainCamBrain.IsBlending);

        yield return new WaitForSeconds(0.2f);

        // currCamera.Priority--;
        // currCamera.gameObject.SetActive(false);
        // currCamera = loadSceneCamera;
        // currCamera.gameObject.SetActive(true);
        // currCamera.Priority++;

        cutsceneDirector.Play();

        yield return new WaitUntil(() => cutsceneDirector.state == PlayState.Paused);

        yield return new WaitForSeconds(0.75f);

        subtitlePanel.SetActive(false);

        GameManager.Instance.ChangeScene("LevelScene");
    }
}
