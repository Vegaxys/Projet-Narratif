using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys
{
    public class EntityBar :MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private BaseCharacter target;

        #endregion


        #region Private Fields

        private IEntity entity;
        public Vector3 offset = new Vector3(0, 35, 0);
        private TextMeshProUGUI pseudo;
        private Image lifeBar;
        private Image shield;

        #endregion


        #region MonoBehaviour Callbacks

        private void Start() {
            lifeBar = transform.GetChild(1).GetComponent<Image>();
            shield = transform.GetChild(2).GetComponent<Image>();
            pseudo = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            if (target != null) {
                FollowTarget();
                UpdateInfo();
            } else {
                Destroy(this.gameObject);
                return;
            }
        }

        #endregion


        #region Public Methods

        public void SetTarget(BaseCharacter _target) {
            // Cache references for efficiency
            target = _target;
            entity = target.GetComponent<IEntity>();
        }

        #endregion


        #region Private Methods

        private void UpdateInfo() {
            lifeBar.fillAmount = (float)entity.GetLife() / (float)entity.GetMaxLife();
            shield.fillAmount = (float)entity.GetShield() / (float)entity.GetMaxShield();
        }

        private void FollowTarget() {
            transform.position = Camera.main.WorldToScreenPoint(target.anchor.position) + offset;
        }

        #endregion
    }
}
