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
        //ship = new Ship();
    }

    /*public void SendJSON(Ship ship)
    {
        string stringListAsJson = JsonUtility.ToJson(ship);

        var req = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"LastUsedShip", stringListAsJson}
            }
        };

        PlayFabClientAPI.UpdateUserData(req, result => Debug.Log("Data sent successfully !"), OnError);
    }

    public void LoadJSON()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), 
        result=>{
            Debug.Log("Received JSON Data.");

            JsonUtility.FromJsonOverwrite(result.Data["LastUsedShip"].Value, ship);

            if (result.Data != null && result.Data.ContainsKey("LastUsedShip"))
            {
                Debug.Log("JSON LOAD : " + ship.stats.name);
            }
        }, OnError);
    }

    public Ship GetShip()
    {
        return ship;
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

    void OnError(PlayFabError e)
    {
        Debug.Log("Error : " + e.GenerateErrorReport());
    }
}