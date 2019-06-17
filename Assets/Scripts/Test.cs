using UnityEngine.UI; 
using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace Vegaxys
{
    public class Test :MonoBehaviour
    {
        public Toggle isConnectedToggle;

        private void Start() {
            StartCoroutine(IsConnected());
        }
        private IEnumerator IsConnected() {
            print(PhotonNetwork.IsConnected);
            isConnectedToggle.isOn = PhotonNetwork.IsConnected;
            yield return new WaitForSeconds(.1f);
            StartCoroutine(IsConnected());
        }
    }
}
