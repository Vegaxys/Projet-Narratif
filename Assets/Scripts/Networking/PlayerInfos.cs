using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class PlayerInfos :MonoBehaviour
    {

        #region Public Fields

        public static PlayerInfos instance;
        public GameObject[] characters;
        public int characterID;

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
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start() {
            if (PlayerPrefs.HasKey("CharacterID")) {
                characterID = PlayerPrefs.GetInt("CharacterID");
            } else {
                characterID = 0;
                PlayerPrefs.SetInt("CharacterID", characterID);
            }
        }

        #endregion
    }
}
