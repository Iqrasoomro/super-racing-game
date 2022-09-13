using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.GL.Abstract;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework.GL.Systems
{

    public class EnemyController : GameSystem
    {
        public static EnemyController Instance;

        public override void Init()
        {
            Instance = this;
        }

        public override void StartSystem()
        {
            EntitiesManager.Instance.enemy.Spawn();
        }

        public override void StopSystem()
        {
            EntitiesManager.Instance.enemy.IsActive = false;
        }

        public override void ClearSystem()
        {
            EntitiesManager.Instance.enemy.Despawn();
        }


    }
}
