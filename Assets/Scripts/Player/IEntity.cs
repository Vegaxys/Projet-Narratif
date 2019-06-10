using UnityEngine;

public interface IEntity {

    void AddLife(int amount);

    void RemoveLife(int amount);

    void AddShield(int amount);

    void RemoveShield(int amount);

    int GetShield();

    int GetLife();

    int GetMaxLife();

    int GetMaxShield();

    Transform GetTransform();
}
