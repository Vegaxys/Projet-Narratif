using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class MenuManager :MonoBehaviour
    {
        #region MonoBehaviour Callbacks

        public void OnCharacterPick(int index) {
            if (PlayerInfos.instance != null) {
                PlayerInfos.instance.characterID = index;
                PlayerPrefs.SetInt("CharacterID", index);
            }
        }

        #endregion
    }
}
