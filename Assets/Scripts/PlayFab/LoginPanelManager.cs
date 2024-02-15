using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.GroupsModels;
using PlayFab.MultiplayerModels;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LoginPanelManager : MonoBehaviour
{
    [SerializeField] private MenuTransitionManager menuTransitionManager;
    [SerializeField] private CinemachineVirtualCamera menuCamera;
    [SerializeField] private MainSceneManager mainSceneManager;
    [SerializeField] TMP_InputField if_userid, if_password;
    [SerializeField] GameObject menuPage;

    private List<TMP_InputField> fields;
    private int fieldIndexer;

    void Awake()
    {
        fields = new List<TMP_InputField>{if_userid, if_password};
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].isFocused)
                {
                    fieldIndexer = i + 1;
                }
            }

            if (fields.Count <= fieldIndexer)
            {
                fieldIndexer = 0;
            }

            fields[fieldIndexer].Select();
            fieldIndexer++;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnButtonLogin();
        }
    }
    
    void OnError(PlayFabError e)
    {
        mainSceneManager.SetStatusText(e.ErrorMessage);
    }

    public void OnButtonLogin()
    {
        if (if_userid.text != "")
        {
            if (if_userid.text.Contains("@"))
            {
                try
                {
                    var loginReq = new LoginWithEmailAddressRequest
                    {
                        Email = if_userid.text,
                        Password = if_password.text,

                        // To get Player Profile, to get Display Name
                        InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                        {
                            GetPlayerProfile = true,
                            GetUserAccountInfo = true,
                            ProfileConstraints = new PlayerProfileViewConstraints()
                            {
                                ShowDisplayName = true
                            }
                        }
                    };

                    PlayFabClientAPI.LoginWithEmailAddress(loginReq, OnLoginSuccess, OnError);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            else
            {
                try
                {
                    var loginReq = new LoginWithPlayFabRequest
                    {
                        Username = if_userid.text,
                        Password = if_password.text,

                        // To get Player Profile, to get Display Name
                        InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                        {
                            GetPlayerProfile = true,
                            GetUserAccountInfo = true,
                            ProfileConstraints = new PlayerProfileViewConstraints()
                            {
                                ShowDisplayName = true
                            }
                        }
                    };

                    PlayFabClientAPI.LoginWithPlayFab(loginReq, OnLoginSuccess, OnError);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }

    public void OnButtonGuestLogin()
    {
        var loginReq = new LoginWithPlayFabRequest
        {
            Username = "Guest",
            Password = "guestpass",

            // To get Player Profile, to get Display Name
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetUserAccountInfo = true,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true
                }
            }
        };

        PlayFabClientAPI.LoginWithPlayFab(loginReq, OnLoginSuccess, OnError);
    }

    public void OnButtonDeviceLogin()
    {
        string customGUID = PlayerPrefs.GetString("DeviceID", Guid.NewGuid().ToString());
        PlayerPrefs.SetString("DeviceID", customGUID);

        var loginReq = new LoginWithCustomIDRequest
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = customGUID,

            // To get Player Profile, to get Display Name
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetUserAccountInfo = true,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true
                }
            }
        };

        PlayFabClientAPI.LoginWithCustomID(loginReq, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult r)
    {
        if (r.InfoResultPayload.PlayerProfile.DisplayName != null && r.InfoResultPayload.PlayerProfile.DisplayName != "")
        {
            mainSceneManager.SetStatusText("Welcome back " + r.InfoResultPayload.PlayerProfile.DisplayName + " !");
            
            PlayFabManager.currPlayFabID = r.PlayFabId;
            PlayFabManager.currTitleID = r.InfoResultPayload.AccountInfo.TitleInfo.TitlePlayerAccount.Id;
            PlayFabManager.currPlayFabDN = r.InfoResultPayload.PlayerProfile.DisplayName;

            //mainSceneManager.SetDisplayName(PlayFabManager.currPlayFabDN);
        }
        else
        {
            string customGUID = PlayerPrefs.GetString("DeviceID");
            string displayName = SafeSubstring(customGUID, 0, 25);

            var req = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = displayName
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(req,
            result=>
            {
                Debug.Log("Device Display Name Set.");
                mainSceneManager.SetStatusText("Welcome back " + result.DisplayName + " !");
            
                PlayFabManager.currPlayFabID = customGUID;
                PlayFabManager.currPlayFabDN = result.DisplayName;

                //mainSceneManager.SetDisplayName(PlayFabManager.currPlayFabDN);
            }, OnError);
        }

        PlayFabGroupsAPI.ListMembership(new ListMembershipRequest
        {

        }, result=>
        {
            if (result.Groups.Count > 0)
            {
                PlayFabManager.currGuildID = result.Groups[0].Group;
                PlayFabManager.currGuildName = result.Groups[0].GroupName;

                PlayFabGroupsAPI.ListGroupMembers(new ListGroupMembersRequest
                {
                    Group = PlayFabManager.currGuildID
                }, mlResult=>
                {
                    foreach (var role in mlResult.Members)
                    {
                        foreach (var member in role.Members)
                        {
                            if (member.Lineage["master_player_account"].Id == PlayFabManager.currPlayFabID)
                            {
                                PlayFabManager.currGuildRole = role.RoleName;

                                Debug.Log(PlayFabManager.currGuildID + " | " + PlayFabManager.currGuildName + " | " + PlayFabManager.currGuildRole);
                            }
                        }
                    }
                }, OnError);
            }
        }, OnError);

        StopAllCoroutines();
        menuTransitionManager.UpdateCamera(menuCamera);
    }

    public static string SafeSubstring(string input, int startIndex, int length)
    {
        if (input.Length >= (startIndex + length))
        {
            return input.Substring(startIndex, length);
        }
        else
        {
            if (input.Length > startIndex)
            {
                return input.Substring(startIndex);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
