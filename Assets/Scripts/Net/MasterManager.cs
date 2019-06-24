using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys
{
    [CreateAssetMenu(menuName = "Singletons/MasterManager")]
    public class MasterManager :ScriptableSingletonObject<MasterManager>
    {
        [SerializeField] private GameSettings _gameSettings;
        public static GameSettings GameSettings { get { return Instance._gameSettings; } }
    }
}
