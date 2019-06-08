
public interface IEntity {

    void AddLife(float amount);

    void RemoveLife(float amount);

    void RemoveEnergy(float amount);

    void AddEnergy(float amount);

    float GetEnergy();

    float GetLife();

    float GetMaxLife();

    float GetMaxEnergy();

    string GetPlayerName();
}
