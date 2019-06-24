using UnityEngine;

namespace Vegaxys
{
    [CreateAssetMenu(menuName = "Singletons/GameSettings")]
    public class GameSettings :ScriptableObject
    {
        [SerializeField] private string _gameVersion = "0.0.0";
        public string GameVersion { get { return _gameVersion; } }

        [SerializeField] private string _playerName = "Vegaxys";
        public string PlayerName { get { return _playerName + "#" + Random.Range(0, 10000); } }
    }
}

