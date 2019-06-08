using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class Register : MonoBehaviour{

    public InputField username;
    public InputField password;
    public InputField confirmPassword;
    public InputField email;

    public void CreateAccount() {
        if(password.text == confirmPassword.text) {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();

            request.Username = username.text;
            request.Password = password.text;
            request.Email = email.text;
            request.DisplayName = username.text;

            PlayFabClientAPI.RegisterPlayFabUser(request, result => {
                Alerts alert = new Alerts();
                StartCoroutine(alert.CreateNewAlert(result.Username + " has been created!"));
            }, error => {
                Alerts alert = new Alerts();
                StartCoroutine(alert.CreateNewAlert(error.ErrorMessage));
            });
        }
    }
}
