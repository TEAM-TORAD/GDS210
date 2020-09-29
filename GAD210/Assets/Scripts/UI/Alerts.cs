using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Alerts : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public IEnumerator LoadSceneAsync(string title, string message)
    {
        // Wait for the new scene to load, then set the message in the new scene.
        yield return SceneManager.LoadSceneAsync("Alerts", LoadSceneMode.Additive);
        GameObject.FindGameObjectWithTag("AlertTitle").transform.GetComponent<Text>().text = title;
        GameObject.FindGameObjectWithTag("AlertMessage").transform.GetComponent<Text>().text = message;
    }
}
