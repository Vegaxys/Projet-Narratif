using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBar : MonoBehaviour{

    public Transform target;
    public float refreshrate;

    private IEntity entity;
    private Vector3 offset = new Vector3(0, 35, 0);

    private TextMeshProUGUI pseudo;
    private Image lifeBar;
    private Image energyBar;

    private void Start() {
        lifeBar = transform.GetChild(1).GetComponent<Image>();
        energyBar = transform.GetChild(2).GetComponent<Image>();
        pseudo = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        entity = target.GetComponent<IEntity>();
        StartCoroutine(UpdateInfo());
    }

    private void Update() {
        FollowTarget();
    }

    private IEnumerator UpdateInfo() {
        lifeBar.fillAmount = entity.GetLife() / entity.GetMaxLife();
        energyBar.fillAmount = entity.GetEnergy() / entity.GetMaxEnergy();
        pseudo.text = entity.GetPlayerName();
        yield return new WaitForSeconds(refreshrate);
        StartCoroutine(UpdateInfo());
    }

    void FollowTarget() {
        transform.position = Camera.main.WorldToScreenPoint(target.position) + offset;
    }
}
