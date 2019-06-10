
public interface IEntity {

    void AddLife(float amount);

    void RemoveLife(float amount);

    void AddShield(float amount);

    void RemoveShield(float amount);

    float GetShield();

    float GetLife();

    float GetMaxLife();

    float GetMaxShield();

}
