using UnityEngine.UI;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Login : MonoBehaviour{

    public InputField username;
    public InputField password;
    public string nextLevel;

    public void LoginPlayer() {
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();

        request.Username = username.text;
        request.Password = password.text;

        PlayFabClientAPI.LoginWithPlayFab(request, result => {
            Client.client.GetUserData(result.PlayFabId, nextLevel);
        }, error => {
            Alerts alert = new Alerts();
            StartCoroutine(alert.CreateNewAlert(error.ErrorMessage));
        });
    }
}
