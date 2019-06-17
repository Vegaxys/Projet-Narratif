using Knife.PostProcessing;
using System.Collections.Generic;
using UnityEngine;


public class SelectionManager : MonoBehaviour{

    public static SelectionManager selection;
    public Transform selectionTransform;

    private void Awake() {
        selection = this;
    }

    private void Update() {

    }
    public void SelectMousePosition() {
        Transform select = null;

        if(select.tag != "Untagged") {
            if (selectionTransform != null && selectionTransform.GetComponent<OutlineRegister>() != null) {
                Destroy(selectionTransform.GetComponent<OutlineRegister>());
            }
            selectionTransform = select;
            selectionTransform.gameObject.AddComponent<OutlineRegister>();
        }
    }
}
