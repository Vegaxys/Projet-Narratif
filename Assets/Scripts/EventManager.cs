using UnityEngine;

namespace Vegaxys {
    public class EventManager :MonoBehaviour
    {
        public static EventManager instance;
        private void Awake() {
            instance = this;

        }
        #region Set Event

        public void SetEvent(WinConditionEnum condition) {
            Objectif objectif = ObjectifManager.instance.GetObjectif(condition);
            if (objectif != null) {
                AddRow(objectif);
                if (objectif.winCondition.repetition == objectif.winCondition.currentrow) {
                    objectif.complete = true;
                }
                ObjectifManager.instance.SetObjectif(objectif);
            }
        }

        public void AddRow(Objectif objectif) {
            if (!objectif.complete) {
                objectif.winCondition.currentrow++;
            }
        }
        #endregion
    }
}
