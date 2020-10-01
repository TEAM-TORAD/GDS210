﻿using System.Collections;
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
    public string levelToLoad = "";
    public void SubmitCode()
    {
        if(code.text.Length > 0)
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), GetVerificationCodeUserData,
            error =>
            {
                Alerts a = new Alerts();
                StartCoroutine(a.LoadSceneAsync("Error!", error.ErrorMessage));
            });
        }
        else
        {
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Error!", "You need to enter the verification-code that was sent to the email you entered when registring this account. Check your junkmail if you can't find it."));
        }
    }
    void VerifyAccountUpdateUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {"verified", "true"}
        }
        },
        result =>
        {
            SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Verification Success!", "Your account is now successfully verified. Welcome to TORAD!"));
            SceneManager.UnloadSceneAsync("VerifyAccount");
        },
        error => {
            Alerts newAlert = new Alerts(); StartCoroutine(newAlert.LoadSceneAsync("Error veryfying account!", error.ErrorMessage));
        });
    }
    void GetVerificationCodeUserData(GetAccountInfoResult initResult)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = initResult.AccountInfo.PlayFabId,
            Keys = null
        }, result => {
            if (result.Data == null || !result.Data.ContainsKey("verificationCode")) Debug.Log("No verification code found for "+ initResult.AccountInfo.Username + " on the server.");
            else
            {
                // Check if verification code is matching
                if (result.Data["verificationCode"].Value == code.text)
                {
                    // Matching codes
                    VerifyAccountUpdateUserData();
                }
                else
                {
                    Alerts a = new Alerts();
                    StartCoroutine(a.LoadSceneAsync("Verification code missmatch!", "The code you entered is incorrect! Please check your email for a verification code."));
                }
            }
        }, (error) => {

            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Error checking account verification!", error.ErrorMessage));
        });
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
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), initResult => 
        {
            string thisPlayFabId = initResult.AccountInfo.PlayFabId;
            string thisEmail = initResult.AccountInfo.PrivateInfo.Email;
            string thisUser = initResult.AccountInfo.Username;
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = thisPlayFabId,
                Keys = null
            }, result => {
                if (result.Data == null || !result.Data.ContainsKey("verificationCode"))
                {
                    UIManager.startPanel.SetActive(true);
                    Alerts a = new Alerts();
                    StartCoroutine(a.LoadSceneAsync("Error!", "No verification code exists on server! Please contact admin at team.torad@gmail.com"));
                    SceneManager.UnloadSceneAsync("Login");
                    PlayerPrefs.DeleteAll();
                }
                else
                {
                    // Send unverified user to verification scene
                    string thisCode = result.Data["verificationCode"].Value;

                    //Send an email to prompt the user to verify their email address.
                    SendEmail registerEmail = new SendEmail();
                    registerEmail.recipientEmail = thisEmail;
                    registerEmail.subject = "Verify account for TORAD";
                    registerEmail.message = "Welcome to TORAD " + thisUser +
                    ". You need to verify your account to be able to login.\nEnter this code in the game to verify your account: " +
                    thisCode;

                    registerEmail.RegistrationEmail();

                    Alerts a = new Alerts();
                    StartCoroutine(a.LoadSceneAsync("Verification Email Sent", "Please check your email, " + thisEmail + ", for a verification code."));

                }
            }, (error) => {

                Alerts a = new Alerts();
                StartCoroutine(a.LoadSceneAsync("Error checking account verification!", error.ErrorMessage));
            });
        },
        error =>
        {
            Alerts a = new Alerts();
            StartCoroutine(a.LoadSceneAsync("Error!", error.ErrorMessage));
        });


        
    }
    public void CloseVerificationPanel(bool openStartPanel)
    {
        UIManager.startPanel.SetActive(openStartPanel);
        SceneManager.UnloadSceneAsync("VerifyAccount");
    }

    public void VerifyAccountOnPlayFab() // No longer used
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SetVerificationCode", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { verifiedValue = 1 }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, result => {
            SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
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