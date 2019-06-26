using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Vegaxys
{
    [System.Serializable]
    public class PlayerProperties
    {
        public string playerName;
        public int playerID;
        public string avatarName;
        public string className;
        public string playerXP;
        public int avatarID;
    }
    public class PlayerInfos :MonoBehaviour
    {

        #region Public Fields

        public static PlayerInfos instance;
        public ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        public PlayerProperties player;

        #endregion


        #region MonoBehaviour Callbacks

        private void OnEnable() {
            if (instance == null) {
                instance = this;
            } else {
                if (instance != null) {
                    Destroy(instance.gameObject);
                    instance = this;
                }
            }
            DontDestroyOnLoad(gameObject);
        }

        #endregion


        #region Public Methods

        public void SetPlayer() {
            player.playerName = PhotonNetwork.LocalPlayer.NickName;

            if (customProperties.ContainsKey("ChampionName")) {
                player.avatarName = customProperties["ChampionName"].ToString();
            }

            if (customProperties.ContainsKey("ChampionClass")) {
                player.className = customProperties["ChampionClass"].ToString();
            }

            if (customProperties.ContainsKey("ChampionIndex")) {
                player.avatarID = (int)customProperties["ChampionIndex"];
            }
        }

        public PlayerProperties GetPlayer() {
            return player;
        }

        #endregion

    }
}
