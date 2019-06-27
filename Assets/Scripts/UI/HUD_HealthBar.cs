using UnityEngine;
using UnityEngine.UI;

namespace Vegaxys {
    public class HUD_HealthBar :MonoBehaviour
    {
        public int playerID;
        public IEntity entity;
        public Image healthImage;
        public Image shieldImage;

        private float currHealth;
        private float currShield;
        private bool setActive;


        private void Update() {
            if (entity != null) {
                UpdateInfo();
            }
        }

        private void UpdateInfo() {
            healthImage.fillAmount = (float)entity.GetLife() / (float)entity.GetMaxLife();
            shieldImage.fillAmount = (float)entity.GetShield() / (float)entity.GetMaxShield();
        }

        public void Activate(IEntity _entity) {
            entity = _entity;
        }
    }
}
