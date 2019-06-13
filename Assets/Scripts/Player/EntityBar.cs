using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Vegaxys
{
    public class EntityBar :MonoBehaviour
    {
        #region Private Fields

        public Transform target;
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
            pseudo = transform.GetComponentInChildren<TextMeshProUGUI>();
            pseudo.text = entity.GetDisplayedName();
        }

        private void Update() {
            if (target == null) {
                Destroy(gameObject);
            }
            FollowTarget();
            UpdateInfo();
        }

        #endregion


        #region Public Methods

        public void SetTarget(IEntity _target) {
            // Cache references for efficiency
            entity = _target;
            target = entity.GetAnchor();
        }

        #endregion


        #region Private Methods

        private void UpdateInfo() {
            lifeBar.fillAmount = (float)entity.GetLife() / (float)entity.GetMaxLife();
            if (shield != null) {
                shield.fillAmount = (float)entity.GetShield() / (float)entity.GetMaxShield();
            }
        }

        private void FollowTarget() {
            transform.position = Camera.main.WorldToScreenPoint(target.position) + offset;
        }

        #endregion
    }
}
