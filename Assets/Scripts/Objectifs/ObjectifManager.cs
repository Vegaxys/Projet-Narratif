using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Vegaxys {
    public enum WinConditionEnum {
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
    public class WinCondition {
        public WinConditionEnum winCondition;
        [Tooltip("Combien de fois on doit répéter l'event avant de le réaliser")]
        [HideInInspector] public int repetition;
        [HideInInspector] public int currentrow;
    }

    [System.Serializable]
    public class Objectif {
        public string title;
        [TextArea(5, 15)] public string description;
        public int scoreRecompense;
        public WinCondition winCondition;
        public bool isMainObjectif;
        [HideInInspector] public bool complete;
    }

    public class ObjectifManager :MonoBehaviour {
        public static ObjectifManager instance;

        public GameObject panelObjectifMouse;
        public Objectif mainObjectif;
        public Objectif[] subObjectifs;
        public ObjectifContainer[] container;

        private void Awake() {
            instance = this;
            panelObjectifMouse.SetActive(false);
        }

        public void Update() {
            if (panelObjectifMouse.activeInHierarchy) {
                OnMouseInElement();
            }
        }

        public void CreateRandomObjectifs() {
            print("hi");
            List<Objectif> currentObjectif = new List<Objectif>();
            for (int i = 0; i < 4; i++) {
                currentObjectif.Add(CreateAnObjectif(currentObjectif));
            }
            mainObjectif = currentObjectif[0];
            mainObjectif.scoreRecompense *= 2;
            mainObjectif.isMainObjectif = true;
            currentObjectif.RemoveAt(0);

            subObjectifs = currentObjectif.ToArray();

            foreach (var item in container) {
                item.SetTitle();
            }
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
            }else
            if (currObjectif.title.Contains("RAMASSER_X_")) {
                currObjectif.winCondition.repetition = UnityEngine.Random.Range(1, 3); // 5 10
                currObjectif.scoreRecompense = 700 + currObjectif.winCondition.repetition * 10;
                return currObjectif;
            }else
            if (currObjectif.title == "TUER_X_ALLIÉ") {
                currObjectif.winCondition.repetition = UnityEngine.Random.Range(1, 3);
                currObjectif.scoreRecompense = 3000 + currObjectif.winCondition.repetition * 1000;
                return currObjectif;
            }
            currObjectif.winCondition.repetition = 1;
            currObjectif.scoreRecompense = 5000;
            return currObjectif;
        }

        public void OnMouseEnterElement(int index) {
            panelObjectifMouse.SetActive(true);
            if (index == 0) {
                panelObjectifMouse.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mainObjectif.title;
                panelObjectifMouse.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = mainObjectif.description;
                panelObjectifMouse.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Compte : " + mainObjectif.winCondition.currentrow + "/" + mainObjectif.winCondition.repetition;
                panelObjectifMouse.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Récompense : " + mainObjectif.scoreRecompense;

            } else {
                panelObjectifMouse.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subObjectifs[index - 1].title;
                panelObjectifMouse.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = subObjectifs[index - 1].description;
                panelObjectifMouse.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Compte : " + subObjectifs[index - 1].winCondition.currentrow + "/" + subObjectifs[index - 1].winCondition.repetition;
                panelObjectifMouse.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Récompense : " + subObjectifs[index - 1].scoreRecompense;
            }
        }

        private void OnMouseInElement() {
            panelObjectifMouse.transform.position = Input.mousePosition;
        }

        public void OnMouseExitElement() {
            panelObjectifMouse.SetActive(false);
        }

        public Objectif GetObjectif(WinConditionEnum condition) {
            if (mainObjectif.winCondition.winCondition == condition) {
                return mainObjectif;
            }
            foreach (var objectif in subObjectifs) {
                if (objectif.winCondition.winCondition == condition) {
                    return objectif;
                }
            }
            return null;
        }

        public void SetObjectif(Objectif objectif) {
            if (mainObjectif.winCondition.winCondition == objectif.winCondition.winCondition) {
                mainObjectif = objectif;
            }
            for (int i = 0; i < subObjectifs.Length; i++) {
                if (subObjectifs[i].winCondition.winCondition == objectif.winCondition.winCondition) {
                    subObjectifs[i] = objectif;
                    break;
                }
            }

            for (int i = 0; i < subObjectifs.Length; i++) {
                container[i + 1].Refresh(subObjectifs[i].complete);
            }
            container[0].Refresh(mainObjectif.complete);
        }

        private WinConditionEnum GetRandomWinCondition(List<Objectif> _currentObjectif) {
            int enumArray = UnityEngine.Random.Range(0, Enum.GetNames(typeof(WinConditionEnum)).Length);
            WinConditionEnum winCondition = (WinConditionEnum)enumArray;
            for (int i = 0; i < _currentObjectif.Count; i++) {
                if (_currentObjectif[i].winCondition.winCondition == winCondition) {
                    return GetRandomWinCondition(_currentObjectif);
                }
            }
            return winCondition;
        }
    } 
}
