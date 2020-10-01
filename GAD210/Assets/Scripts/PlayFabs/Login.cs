using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    public GameObject loginPanel;
    public TMP_InputField username;
    public TMP_InputField password;
    public Button submissionButton;
    public string levelToLoad;
    public void LoginUser()
    {
        submissionButton.interactable = false;
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
        request.Username = username.text;
        request.Password = password.text;

        PlayFabClientAPI.LoginWithPlayFab(request, result => {
            GetStatistics();
            loginPanel.SetActive(false);
        }, error => {
            submissionButton.interactable = true;
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Login Error", error.ErrorMessage));
        });
    }
    public void OpenResetPasswordPanel()
    {
        loginPanel.SetActive(false);
        SceneManager.UnloadSceneAsync("Login");
        SceneManager.LoadSceneAsync("ResetPassword", LoadSceneMode.Additive);
    }
    public void CloseLoginPanel()
    {
        UIManager.startPanel.SetActive(true);
        SceneManager.UnloadSceneAsync("Login");
    }

    void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        foreach (var eachStat in result.Statistics)
        {
            if(eachStat.StatisticName == "verified")
            {
                if(eachStat.Value == 0)
                {
                    SceneManager.UnloadSceneAsync("Login");
                    SceneManager.LoadSceneAsync("VerifyAccount", LoadSceneMode.Additive);
                }
                else if(eachStat.Value == 1)
                {
                    SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
                }
            }
        }
    }

}
