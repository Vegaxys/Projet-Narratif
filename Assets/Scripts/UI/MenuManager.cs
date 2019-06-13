using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class MenuManager :MonoBehaviour
    {
        #region Public Fields

        public static MenuManager instance;

        #endregion


        #region Private Serializable Fields



        #endregion


        #region MonoBehaviour Callbacks

        private void Awake() {
            instance = this;
        }

        public void OnCharacterPick(int index) {
            if (PlayerInfos.instance != null) {
                PlayerInfos.instance.characterID = index;
                PlayerPrefs.SetInt("CharacterID", index);
            }
        }

        #endregion
    }
}
