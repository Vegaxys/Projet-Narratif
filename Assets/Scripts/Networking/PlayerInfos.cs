using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class PlayerInfos :MonoBehaviour
    {

        #region Public Fields

        public static PlayerInfos instance;
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
    }
}
