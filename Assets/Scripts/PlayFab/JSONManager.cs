using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class JSONManager : MonoBehaviour
{

    void Awake()
    {
        
    }

    public static void SendJSON(int runsCompleted)
    {
        string stringListAsJson = JsonUtility.ToJson(runsCompleted);

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"RunsCompleted", runsCompleted.ToString()}
            }
        }, result=>
        {
            Debug.Log("Data sent successfully !");
        }, OnError);
    }

    /*public int GetRunsCompleted()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), 
        result=>
        {
            Debug.Log("Received JSON Data.");

            JsonUtility.FromJsonOverwrite(result.Data["RunsCompleted"].Value, runsCompleted);

            if (result.Data != null && result.Data.ContainsKey("RunsCompleted"))
            {
                Debug.Log("JSON LOAD : " + result.Data["RunsCompleted"].Value);
                //runsCompleted = int.Parse(result.Data["RunsCompleted"].Value);
            }
        }, OnError);

        return 0;
    }*/

    public static string StringListToJSON(string key, List<string> values)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append("{\"");
        stringBuilder.Append(key);
        stringBuilder.Append("\":[");

        for (int i = 0; i < values.Count; i++)
        {
            string value = values[i];
            stringBuilder.Append("\"");
            stringBuilder.Append(value);
            stringBuilder.Append("\"");

            if (i != values.Count - 1)
            {
                stringBuilder.Append(",");
            }
        }

        stringBuilder.Append("]}");

        return stringBuilder.ToString();
    }

    static void OnError(PlayFabError e)
    {
        Debug.Log("Error : " + e.GenerateErrorReport());
    }
}