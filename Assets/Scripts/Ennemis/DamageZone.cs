﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys
{
    public class DamageZone :MonoBehaviour
    {
        public Ennemi entity;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Projectile")) {
                Projectile projectile = other.GetComponent<Projectile>();
                entity.GetTriggered(projectile);
            }
        }
    }
}
