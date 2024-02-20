using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    // FPS Variables
    public TMP_Text fpsText;
    private int frameIndex;
    private float[] frameDeltaTimeArray;

    void Awake()
    {
        frameDeltaTimeArray = new float[50];
        StartCoroutine(DisplayFPS());
    }

    private IEnumerator DisplayFPS()
    {
        frameDeltaTimeArray[frameIndex] = Time.deltaTime;
        frameIndex = (frameIndex + 1) % frameDeltaTimeArray.Length;

        fpsText.text = "FPS : " + Mathf.RoundToInt(CalculateFPS()).ToString();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(DisplayFPS());
    }

    private float CalculateFPS()
    {
        float total = 0f;

        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }
}
