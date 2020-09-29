using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Register : MonoBehaviour
{
    public Transform registerPanel;
    public InputField username;
    public InputField password;
    public InputField confirmPassword;
    public InputField email;
    public void CreateAccount()
    {
        if (password.text.Length < 3 || confirmPassword.text.Length < 3)
        {
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Short Password", "The password needs to be at lest 3 characters long."));
        }
        else
        {
            if (password.text == confirmPassword.text)
            {
                RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
                request.Username = username.text;
                request.Password = confirmPassword.text;
                request.Email = email.text;
                request.DisplayName = username.text;

                PlayFabClientAPI.RegisterPlayFabUser(request, result => {
                    Alerts a = new Alerts();
                    //registerPanel.gameObject.SetActive(false);
                    StartCoroutine(a.LoadSceneAsync("Successful Registration", "A new user was successfully created. Welcome to the game " + request.Username + "."));
                    CloseRegisterPanel();
                }, error => {
                    Alerts a = new Alerts();
                    StartCoroutine(a.LoadSceneAsync("Registration Error", error.ErrorMessage));
                });
            }
            else
            {
                Alerts a = new Alerts();
                StartCoroutine(a.LoadSceneAsync("Password missmatch", "The password in the second field does not match the password in the first field."));
            }
        }
    }

    public void CloseRegisterPanel()
    {
        UIManager.startPanel.SetActive(true);
        SceneManager.UnloadSceneAsync("Register");
    }
}
