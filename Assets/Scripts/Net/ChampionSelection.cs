using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Vegaxys
{
    public class ChampionSelection :MonoBehaviour
    {
        private ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();

        [SerializeField] private TextMeshProUGUI championLabel;
        
        private void SetChampion(int champion) {
            switch (champion) {
                case 0:
                    championLabel.text = "Exilé";
                    customProperties["ChampionName"] = "Exilé";
                    break;
                case 1:
                    championLabel.text = "Native";
                    customProperties["ChampionName"] = "Native";
                    break;
                case 2:
                    championLabel.text = "Oméga";
                    customProperties["ChampionName"] = "Oméga";
                    break;
                case 3:
                    championLabel.text = "Epsilon";
                    customProperties["ChampionName"] = "Epsilon";
                    break;
                default:
                    championLabel.text = "Null";
                    break;
            }
            customProperties["ChampionIndex"] = champion;
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }

        public void OnClick_SelectChampion(int champion) {
            SetChampion(champion);
        }
    }
}
