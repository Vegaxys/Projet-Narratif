using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Vegaxys {

    public class HUD_Manager :MonoBehaviour {

        public static HUD_Manager manager;

        public Image capa01_Cooldown;
        public Image capa02_Cooldown;
        public TextMeshProUGUI chargeur;

        [Header("Consos")]
        public TextMeshProUGUI consoShieldText;
        public TextMeshProUGUI consoHealthText;
        public TextMeshProUGUI consoGrenadeText;

        public BaseCharacter character;

        public void Awake() {
            manager = this;
        }
        private void Update() {
            
        }

        #region Character HUD 

        public void Update_Capa01_Cooldown(float amount) {
            capa01_Cooldown.fillAmount = amount;
        }

        public void Update_Capa02_Cooldown(float amount) {
            capa02_Cooldown.fillAmount = amount;
        }

        public void Update_Chargeur(int curr, int max, int all) {
            chargeur.text = "Ammo : " + curr + "/" + max + " || " + all;
        }
        public void Update_Consos(int shield, int health, int grenade) {
            consoShieldText.text = "x " + shield;
            consoHealthText.text = "x " + health;
            consoGrenadeText.text = "x " + grenade;
        }

        #endregion
    }
}
