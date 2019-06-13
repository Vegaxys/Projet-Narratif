using Knife.PostProcessing;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OutlineSettings
{
    public string type;
    public Color color;
}

public class SelectionManager : MonoBehaviour{

    public OutlineSettings[] settings;

    public static SelectionManager selection;
    public Transform selectionTransform;

    private void Awake() {
        selection = this;
    }

    private void Update() {
        if (Input.GetButtonDown("Select")) {
            SelectMousePosition();
        }
    }
    private void SelectMousePosition() {
        Transform select = null;
 //       GameManager.gm.MousePosition(out select);

        if(select.tag != "Untagged") {
            if (selectionTransform != null && selectionTransform.GetComponent<OutlineRegister>() != null) {
                Destroy(selectionTransform.GetComponent<OutlineRegister>());
            }
            selectionTransform = select;
            selectionTransform.gameObject.AddComponent<OutlineRegister>();
        }
    }
    public OutlineSettings GetSettings(string type) {
        for (int i = 0; i < settings.Length; i++) {
            if(settings[i].type == type) {
                return settings[i];
            }
        }
        return null;
    }
}
