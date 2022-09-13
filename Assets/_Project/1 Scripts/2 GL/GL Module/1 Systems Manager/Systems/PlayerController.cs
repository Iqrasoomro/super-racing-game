using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.GL.Abstract;
using ArcadianLab.SimFramework.GL.Systems;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework.GL.Systems
{
    public class PlayerController : GameSystem
    {
        //CubeGameData _data;

        public override void Init()
        {
            /*if (!player) player = FindObjectOfType<Player>(); // Temp - red flag
            Assert.IsNotNull(player);*/
        }

        public override void StartSystem()
        {
            EntitiesManager.Instance.player.Spawn();
        }

        public override void StopSystem()
        {
            EntitiesManager.Instance.player.IsActive = false;
        }

        public override void ClearSystem()
        {
            EntitiesManager.Instance.player.Despawn();
        }
    }
}
