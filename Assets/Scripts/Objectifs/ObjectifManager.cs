using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
        TUER_MYRMIDON,
        TUER_MINIBOSS,

        NE_TUER_PERSONNE,
        NE_PAS_MOURRIR,

        RAMASSER_X_GRENADE,
        RAMASSER_X_MUNITION,
        RAMASSER_X_HEAL,
        RAMASSER_X_SHIELD,

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
        public WinCondition winCondition;
        public bool isMainObjectif;
    }

    public class ObjectifManager :MonoBehaviour
    {
        public static ObjectifManager instance;
        public GameObject panelObjectifMouse;
        public Objectif mainObjectif;
        public Objectif[] subObjectifs;

        private void Awake() {
            instance = this;
            panelObjectifMouse.SetActive(false);
        }

        public void CreateRandomObjectifs() {
            List<Objectif> currentObjectif = new List<Objectif>();
            for (int i = 0; i < 4; i++) {
                currentObjectif.Add(CreateAnObjectif(currentObjectif));
            }
            mainObjectif = currentObjectif[0];
            mainObjectif.scoreRecompense *= 2;
            mainObjectif.isMainObjectif = true;
            currentObjectif.RemoveAt(0);

            subObjectifs = currentObjectif.ToArray();
        }

        private Objectif CreateAnObjectif(List<Objectif> currentObjectif) {

            Objectif currObjectif = new Objectif();

            WinConditionEnum condition = GetRandomWinCondition(currentObjectif);

            WinCondition _winCondition = new WinCondition();
            _winCondition.winCondition = condition;
            currObjectif.winCondition = _winCondition;

            currObjectif.title = currObjectif.winCondition.winCondition.ToString();
            List<string> yay = new List<string>();
            for (int i = 0; i < UnityEngine.Random.Range(5, 40); i++) {
                yay.Add("AYAYA ");
            }
            currObjectif.description = string.Concat(yay);

            if (currObjectif.title.Contains("TUER_X_")) {
                currObjectif.winCondition.repetition = UnityEngine.Random.Range(10, 30);
                currObjectif.scoreRecompense = 1500 + currObjectif.winCondition.repetition * 10;
            }
            if (currObjectif.title.Contains("RAMASSER_X_")) {
                currObjectif.winCondition.repetition = UnityEngine.Random.Range(5, 15);
                currObjectif.scoreRecompense = 700 + currObjectif.winCondition.repetition * 10;
                return currObjectif;
            }
            if (currObjectif.title.Contains("TUER_X_ALLIÉ")) {
                currObjectif.winCondition.repetition = UnityEngine.Random.Range(1, 3);
                currObjectif.scoreRecompense = 3000 + currObjectif.winCondition.repetition * 1000;
                return currObjectif;
            }
            currObjectif.winCondition.repetition = 1;
            currObjectif.scoreRecompense = 5000;
            return currObjectif;
        }

        public void Update() {
            if (panelObjectifMouse.activeInHierarchy) {
                OnMouseInElement();
            }
        }

        public void OnMouseEnterElement(int index) {
            panelObjectifMouse.SetActive(true);
            if (index == 0) {
                panelObjectifMouse.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mainObjectif.title;
                panelObjectifMouse.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = mainObjectif.description;
                panelObjectifMouse.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Récompense : " + mainObjectif.scoreRecompense;

            } else {
                panelObjectifMouse.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subObjectifs[index - 1].title;
                panelObjectifMouse.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = subObjectifs[index - 1].description;
                panelObjectifMouse.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Récompense : " + subObjectifs[index - 1].scoreRecompense;
            }
        }

        private void OnMouseInElement() {
            panelObjectifMouse.transform.position = Input.mousePosition;
        }

        public void OnMouseExitElement() {
            panelObjectifMouse.SetActive(false);
        }

        private WinConditionEnum GetRandomWinCondition(List<Objectif> _currentObjectif) {
            int enumArray = UnityEngine.Random.Range(0, Enum.GetNames(typeof(WinConditionEnum)).Length);
            WinConditionEnum winCondition = (WinConditionEnum)enumArray;
            for (int i = 0; i < _currentObjectif.Count; i++) {
                if(_currentObjectif[i].winCondition.winCondition == winCondition) {
                    return GetRandomWinCondition(_currentObjectif);
                }
            }
            return winCondition;
        }
    }
}
