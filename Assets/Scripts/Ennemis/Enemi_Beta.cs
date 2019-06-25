using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys
{
    public class Enemi_Beta :Ennemi
    {
        private int lifeShield = 150;
        public GameObject shieldObject;

        public override void TakeDamage(int amount) {
            if (lifeShield > 0) {
                lifeShield -= amount;
                return;
            }
            if (lifeShield < 0) {
                Destroy(shieldObject);
                lifeShield = 0;
            }
            base.TakeDamage(amount);
        }
    }
}
