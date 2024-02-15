using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabInitial : MonoBehaviour
{
    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            // Title ID
            PlayFabSettings.staticSettings.TitleId = "DCAAB";
        }

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.DeveloperSecretKey))
        {
            // Developer Secret
            PlayFabSettings.staticSettings.DeveloperSecretKey = "I4XWH5YBNZHIFMBJ1MCRS3Y8X81IOMZXOYF8TONNMBTOYKBB1B";
        }
    }
}
