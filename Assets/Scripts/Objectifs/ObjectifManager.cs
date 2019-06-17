using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public enum WinConditionEnum
    {
        TUER_TANK,
        TUER_SOUTIENT,
        TUER_ASSASSIN,
        TUER_TIREUR,
        TUER_X_ALLIÉ,
        TUER_X_LAMBDA,
        TUER_X_ALPHA,
        TUER_X_BETA,
        TUER_X_MYRMIDON,
        TUER_X_MINIBOSS,

        NE_TUER_PERSONNE,
        NE_PAS_MOURRIR,

        RAMMASSER_X_GRENADE,
        RAMMASSER_X_MUNITION,
        RAMMASSER_X_SHIELD,

        AVOIR_MAX_XP,
        AVOIR_MAX_KILL_IA,
    }

    [System.Serializable]
    public class WinCondition
    {
        public WinConditionEnum winCondition;
        [Tooltip("Combien de fois on doit répéter l'event avant de le réaliser")]
        public int repetition;
    }

    [System.Serializable]
    public class Objectif
    {
        public string title;
        [TextArea(5, 15)] public string description;
        public int scoreRecompense;
        public WinCondition[] winConditions;

    }

    public class ObjectifManager :MonoBehaviour
    {
        public Objectif mainObjectif;
        public Objectif[] subObjectifs;
    }
}
