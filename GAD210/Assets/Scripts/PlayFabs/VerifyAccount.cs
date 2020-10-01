using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VerifyAccount : MonoBehaviour
{
    public TMP_InputField code;
    public string sceneToLoad = "";
    public void SubmitCode()
    {
        if(code.text.Length > 0)
        {
            PlayFabClientAPI.GetPlayerStatistics(
                new GetPlayerStatisticsRequest(),
                OnGetStatistics,
                error => Debug.LogError(error.GenerateErrorReport())
            );
        }
        else
        {
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Error!", "You need to enter teh verification-code that was sent to the email you entered when registring this account. Check your junkmail if you can't find it."));
        }
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        foreach (var eachStat in result.Statistics)
        {
            if (eachStat.StatisticName == "verification code")
            {
                if (eachStat.Value.ToString() == code.text)
                {
                    VerifyAccountOnPlayFab();
                }
                else
                {
                    Alerts a = new Alerts();
                    StartCoroutine(a.LoadSceneAsync("Error!", "The code you entered is incorrect!"));
                }
            }
        }
    }
    public void ResendCode()
    {
        Debug.Log("Function to resend code is not created yet.");
    }
    public void CloseVerificationPanel(bool openStartPanel)
    {
        UIManager.startPanel.SetActive(openStartPanel);
        SceneManager.UnloadSceneAsync("VerifyAccount");
    }

    public void VerifyAccountOnPlayFab()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SetVerificationCode", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { verifiedValue = 1 }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, result => {
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Successful Verification", "Your account is now successfully verified. Welcome to TORAD ninja!"));
            CloseVerificationPanel(false);
        }, error => {
            Alerts newAlert = new Alerts(); 
            StartCoroutine(newAlert.LoadSceneAsync("Error verifying account!", error.ErrorMessage + " \nPlease copy this error message and send it to team.torad@gmail.com along with your username. \n We appologise for any inconvenience."));
            CloseVerificationPanel(true);
        });
    }
}
