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
    public string levelToLoad;
    public void LoginUser()
    {
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
        request.Username = username.text;
        request.Password = password.text;

        PlayFabClientAPI.LoginWithPlayFab(request, result => {
            SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
            loginPanel.SetActive(false);
        }, error => {
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Login Error", error.ErrorMessage));
        });
    }
    public void OpenResetPasswordPanel()
    {
        loginPanel.SetActive(false);
        SceneManager.LoadSceneAsync("ResetPassword", LoadSceneMode.Additive);
    }
    public void CloseLoginPanel()
    {
        UIManager.startPanel.SetActive(true);
        SceneManager.UnloadSceneAsync("Login");
    }
}
