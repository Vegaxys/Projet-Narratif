using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    [System.Serializable]
    public class Spell
    {
        public string title;
        public string description;
        public int capaIndex;
    }
    public class Capa :MonoBehaviourPunCallbacks
    {
        #region Public Fields
        public Spell current_Spell;
        public GameObject capa_Gizmos;
        public float capa_Cooldown;
        public float capa_Loading;
        public float capa_Range;
        public float capa_GizAOE_Range;
        public bool capa_NeedTarget;
        public bool capa_NeedAOE;
        public bool capa_Ready = true;

        [HideInInspector] public Transform capa_Target;
        [HideInInspector] public BaseCharacter character;

        #endregion


        #region Private Fields

        private HUD_Manager hud;
        [HideInInspector] public PhotonView view;

        #endregion


        #region Private Serializable Fields


        #endregion


        #region MonoBehaviour CallBacks

        public virtual void Start() {
            hud = HUD_Manager.manager;
            character = GetComponentInParent<BaseCharacter>();
            view = GetComponent<PhotonView>();
        }

        private void Update() {
            if (Input.GetButtonDown("Capa0" + current_Spell.capaIndex) && capa_Ready) {
                if(capa_Gizmos != null) {
                    capa_Gizmos.SetActive(true);
                    capa_Gizmos.transform.localScale = Vector3.one * capa_Range;
                    GameManager.instance.gizAOE.SetActive(capa_NeedAOE);
                    GameManager.instance.gizAOE.transform.localScale = Vector3.one * capa_GizAOE_Range;
                }
            }
            if (Input.GetButton("Capa0" + current_Spell.capaIndex)) {
                if (capa_NeedTarget) {
                    Virtual_GetTarget();
                }
                if (capa_NeedAOE) {
                    Virtual_Gizmos_AOE();
                }
            }
            if (Input.GetButtonUp("Capa0" + current_Spell.capaIndex) && capa_Ready) {
                print(current_Spell.capaIndex);
                GameManager.instance.gizAOE.SetActive(false);
                if (capa_Gizmos != null) {
                    capa_Gizmos.SetActive(false);
                    if (capa_NeedTarget && capa_Target == null) {
                        return;
                    }
                }
                view.RPC("RPC_Virtual_Launch_Spell", RpcTarget.All);
                StartCoroutine(RecoverCapa());
            }
        }

        #endregion


        #region Virtuals Methods 

        [PunRPC]
        public virtual void RPC_Virtual_Launch_Spell() {
            print(current_Spell.title + " has been launched !! ");
            Virtual_DeselectAllTargets();
        }

        public virtual void Virtual_Gizmos_AOE() {
            GameManager.instance.gizAOE.transform.position = GameManager.instance.MousePosition(capa_Range, transform.position);
        }

        public virtual void Virtual_GetTarget() {
            if (Input.GetButtonDown("Select")) {
                IEntity entity = GameManager.instance.GetEntity(capa_Range, transform.position);
                if (entity != null) {
                    if (capa_Target != null) {
                        Virtual_DeselectAllTargets();
                    }
                    capa_Target = entity.GetTransform();
                }
            }
        }

        public virtual void Virtual_DeselectAllTargets() {
            if (!capa_Target)
                return;
            GameManager.instance.DeselectTarget(capa_Target);
            capa_Target = null;
        }

        #endregion


        #region Public Methods

        public IEnumerator RecoverCapa() {
            capa_Ready = false;
            float t = 0;
            while (t < capa_Cooldown) {
                t += Time.deltaTime;
                if (current_Spell.capaIndex == 1) hud.Update_Capa01_Cooldown(t / capa_Cooldown);
                if (current_Spell.capaIndex == 2) hud.Update_Capa02_Cooldown(t / capa_Cooldown);
                yield return null;
            }
            capa_Ready = true;
        }

        #endregion
    }
}
