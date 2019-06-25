using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Vegaxys {

    public class HUD_Manager :MonoBehaviour {

        public static HUD_Manager manager;

        public Image capa01_Cooldown;
        public Image capa02_Cooldown;
        public TextMeshProUGUI scoreText;

        [Header("Consos")]
        public TextMeshProUGUI consoShieldText;
        public TextMeshProUGUI consoHealthText;
        public TextMeshProUGUI consoGrenadeText;

        [Header("Arme principale")]
        [SerializeField] private Image chargeurImage;
        [SerializeField] private TextMeshProUGUI chargeurText;

        [HideInInspector] public BaseCharacter character;

        public void Awake() {
            manager = this;
        }

        #region Character HUD 

        public void Update_Capa01_Cooldown(float amount) {
            capa01_Cooldown.fillAmount = amount;
        }

        public void Update_Capa02_Cooldown(float amount) {
            capa02_Cooldown.fillAmount = amount;
        }

        public void Update_Chargeur(int curr, int max, int all) {
            chargeurText.text = curr + "/" + all;
            chargeurImage.fillAmount = (2 / 3) / (curr / max);
        }

        public void Update_Consos(int shield, int health, int grenade) {
            consoShieldText.text = "x " + shield;
            consoHealthText.text = "x " + health;
            consoGrenadeText.text = "x " + grenade;
        }

        public void Update_Capacites() {
            capa01_Cooldown.GetComponentInChildren<TextMeshProUGUI>().text = character.capa01.current_Spell.title;
            capa02_Cooldown.GetComponentInChildren<TextMeshProUGUI>().text = character.capa02.current_Spell.title;
        }

        public void Update_Score(int amount) {
            scoreText.text = "Score : " + amount;
        }

        #endregion
    }
}
