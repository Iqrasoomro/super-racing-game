using UnityEngine;
using System;

namespace Cube.MiniGame.Data
{
    [CreateAssetMenu(fileName = "CubeGameData", menuName = "CubeGame/CubeGameData")]
    public class CubeGameData : ScriptableObject
    {
        public Game game;
        public Player player;
        public Coin coin;
        public Hurdle hurdle;

        [Serializable]
        public class Game
        {
            public int MaxCoinsPerLevel;
        }

        [Serializable]
        public class Player
        {
            public int SpawnPositionY;
            public int SwerveSpeed;
            public int SwerveLimitX;
        }

        [Serializable]
        public abstract class Spawn
        {
            public Vector2Int SpawnInterval;
            public int SpawnPositionY;
            public int SpawnRangeX;
        }

        [Serializable]
        public class Coin : Spawn 
        {
            public int ScorePerCoin;
        }

        [Serializable]
        public class Hurdle : Spawn { }

    }
}

