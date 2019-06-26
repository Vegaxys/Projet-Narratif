using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Vegaxys
{
    public class ChampionSelection :MonoBehaviour
    {
        private void SetChampion(int champion) {
            switch (champion) {
                case 0:
                    PlayerInfos.instance.customProperties["ChampionName"]= "Exilé";
                    PlayerInfos.instance.customProperties["ChampionClass"] = "Tireur";
                    break;
                case 1:
                    PlayerInfos.instance.customProperties["ChampionName"] = "Native";
                    PlayerInfos.instance.customProperties["ChampionClass"] = "Tank";
                    break;
                case 2:
                    PlayerInfos.instance.customProperties["ChampionName"] = "Oméga";
                    PlayerInfos.instance.customProperties["ChampionClass"] = "Support";
                    break;
                case 3:
                    PlayerInfos.instance.customProperties["ChampionName"] = "Epsilon";
                    PlayerInfos.instance.customProperties["ChampionClass"] = "Assassin";
                    break;
                default:
                    break;
            }
            PlayerInfos.instance.customProperties["ChampionIndex"] = champion;
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerInfos.instance.customProperties);
            PlayerInfos.instance.SetPlayer();
        }

        public void OnClick_SelectChampion(int champion) {
            SetChampion(champion);
        }
    }
}
