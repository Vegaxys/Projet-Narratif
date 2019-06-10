using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD_Manager : MonoBehaviour{

    public static HUD_Manager manager;

    public TextMeshProUGUI currentBullet;

    private void Awake() {
        manager = this;
    }

    public void RefreshMunitionDisplay(int current, int max) {
        currentBullet.text = current + " / " + max;
    }
}
