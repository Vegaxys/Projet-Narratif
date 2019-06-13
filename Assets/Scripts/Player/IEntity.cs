using UnityEngine;

public interface IEntity {

    int GetShield();

    int GetLife();

    int GetMaxLife();

    int GetMaxShield();

    string GetDisplayedName();

    Transform GetTransform();

    Transform GetAnchor();
}
