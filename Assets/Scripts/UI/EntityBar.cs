﻿using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
        private Image reloadingImage;
        private Player player;

        #endregion


        #region MonoBehaviour Callbacks

        private void Start() {
            lifeBar = transform.GetChild(1).GetComponent<Image>();
            shield = transform.GetChild(2).GetComponent<Image>();
            pseudo = transform.GetComponentInChildren<TextMeshProUGUI>();
            foreach (var item in PhotonNetwork.PlayerList) {
                if (item.ActorNumber == entity.GetTransform().GetComponent<PhotonView>().Owner.ActorNumber) {
                    player = item;
                    break;
                }
            }
            pseudo.text = entity.GetDisplayedName();
            reloadingImage = transform.GetChild(4).GetComponent<Image>();
            if (reloadingImage != null) reloadingImage.gameObject.SetActive(false);
        }

        private void Update() {
            if (Camera.main != null) {
                FollowTarget();
                UpdateInfo();
            }
            if (target == null) {
                Destroy(gameObject);
            }
        }

        #endregion


        #region Public Methods

        public void SetTarget(IEntity _target) {
            // Cache references for efficiency
            entity = _target;
            target = entity.GetAnchor();
        }

        public IEnumerator Reloading(float speed) {
            reloadingImage.gameObject.SetActive(true);
            float t = 0;
            while (t < speed) {
                t += Time.deltaTime;
                reloadingImage.fillAmount = t / speed;
                yield return null;
            }
            reloadingImage.gameObject.SetActive(false);
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
            if(target != null)
                transform.position = Camera.main.WorldToScreenPoint(target.position) + offset;
        }

        #endregion
    }
}
