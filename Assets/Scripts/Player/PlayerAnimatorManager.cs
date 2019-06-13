using UnityEngine;
using System.Collections;

namespace Vegaxys
{
    public class PlayerAnimatorManager :MonoBehaviour
    {
        #region Private Fields

        private Animator animator;

        #endregion

        #region Private Serializable Fields

        [SerializeField] private float directionDampTime = 0.25f;

        #endregion


        #region MonoBehaviour Callbacks

        private void Start() {
            animator = GetComponent<Animator>();
            if (!animator) {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        private void Update() {
            if (!animator) {
                return;
            }
            float xx = Input.GetAxis("Horizontal");
            float zz = Input.GetAxis("Vertical");
          /*  if (zz < 0) {
                zz = 0;
            }*/
            animator.SetFloat("Speed", xx * xx + zz * zz);
            animator.SetFloat("Direction", xx, directionDampTime, Time.deltaTime);
        }
        #endregion
    }
}
