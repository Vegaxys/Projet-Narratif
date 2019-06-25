using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Vegaxys {
    public class ObjectifContainer :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int objectif;

        public Color panelColorNormal;
        public Color panelColorComplete;
        public Color panelColorFailed;


       /* public void Start() {
            Invoke("SetTitle", .8f);
        }*/
        public void SetTitle() {
            if (objectif == 0) {
                transform.GetComponentInChildren<TextMeshProUGUI>().text = ObjectifManager.instance.mainObjectif.title;
            } else {
                transform.GetComponentInChildren<TextMeshProUGUI>().text = ObjectifManager.instance.subObjectifs[objectif - 1].title;
            }
        }

        public void Refresh(bool complete) {
            if (complete) {
                GetComponent<Image>().color = panelColorComplete;
            } else{
                GetComponent<Image>().color = panelColorNormal;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            ObjectifManager.instance.OnMouseEnterElement(objectif);
        }

        public void OnPointerExit(PointerEventData eventData) {
            ObjectifManager.instance.OnMouseExitElement();
        }
    }
}
