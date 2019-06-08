using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Alerts
{
    public IEnumerator CreateNewAlert(string message) {
        yield return SceneManager.LoadSceneAsync("Alerts", LoadSceneMode.Additive);

       GameObject.FindGameObjectWithTag("AlertMessage").GetComponent<AlertObjects>().alertText.text = message;
    }
}
