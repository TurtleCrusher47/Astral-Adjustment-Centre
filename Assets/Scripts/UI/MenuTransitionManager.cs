using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class MenuTransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera currCamera;
    [SerializeField] private CinemachineVirtualCamera menuCamera;

    void Awake()
    {
        currCamera.Priority++;

        StartCoroutine(TransitionToMenu());
    }

    private IEnumerator TransitionToMenu()
    {
        yield return new WaitForSeconds(1);

        UpdateCamera(menuCamera);
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currCamera.Priority--;
        currCamera = target;
        currCamera.Priority++;
    }
}
