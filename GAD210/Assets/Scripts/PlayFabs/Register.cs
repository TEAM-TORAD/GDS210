using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab.Json;

public class Register : MonoBehaviour
{
    public Transform registerPanel;
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField confirmPassword;
    public TMP_InputField email;

    public static Register register;

    private void Awake()
    {
        if(register == null)
        {
            register = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private int GenerateVerificationCode()
    {
        System.Random r = new System.Random();
        string randomNR = "";
        for (int i = 1; i <= 6; ++i)
        {
            // First and last digit can't be 0, else it will be removed once the string gets converted to an int
            if(i == 1 || i == 6) randomNR += r.Next(1, 9).ToString();
            else randomNR += r.Next(0, 9).ToString();
        }
        return int.Parse(randomNR);
    }
    private void SetVerificationCode(string username, string password, int verCode)
    {
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
        request.Username = username;
        request.Password = password;

        PlayFabClientAPI.LoginWithPlayFab(request, result => {
            // If the player was successfully logged in, set the verification code and set the status of the player to verified = 0 (not verified account)
            SetCode(verCode);
        }, error => {
            Debug.Log("Could not login to newly created user (in process of sending a verification email.");
        });
    }
    // Build the request object and access the API
    public void SetCode(int verCode)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SetVerificationCode", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { verificationCode = verCode, verifiedValue = 0 }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, result => { 
            //Send an email to prompt the user to verify their email address.
            SendEmail registerEmail = new SendEmail();
            registerEmail.recipientEmail = email.text;
            registerEmail.subject = "Verify account for TORAD";
            registerEmail.message = "Welcome to TORAD " + username.text +
            ". You need to verify your account to be able to login.\nEnter this code in the game to verify your account: " +
            verCode;

            registerEmail.RegistrationEmail();

            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Successful Registration", "A new user was successfully created. Welcome to the game " + register.username.text + ".\nPlease check you email for a verification code. You will have to enter the code on your first login."));
            CloseRegisterPanel();
        }, error => {
            Alerts newAlert = new Alerts(); StartCoroutine(newAlert.LoadSceneAsync("Error creating account!", error.ErrorMessage)); 
        });
    }


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

                    // Generate a verification code
                    int verCode = GenerateVerificationCode();
                    SetVerificationCode(username.text, confirmPassword.text, verCode);

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
