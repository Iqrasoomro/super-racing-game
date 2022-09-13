using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.GL.Abstract;
using ArcadianLab.SimFramework.GL.Systems;
using ArcadianLab.SimFramework.GL.Entities;
using Cube.MiniGame.Data;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework.GL.Systems
{
    public class PickablesManager : GameSystem
    {
        public override void Init() => Player.PickablePicked += OnPickablePicked;
        public override void Dispose() => Player.PickablePicked -= OnPickablePicked;

        public override void StartSystem()
        {
            foreach (Pickable pickable in EntitiesManager.Instance.pickables)
            {
                pickable.Spawn();
            }
        }

        public override void StopSystem()
        {
            foreach (Pickable pickable in EntitiesManager.Instance.pickables)
            {
                pickable.IsActive = false;
            }
        }

        public override void ClearSystem()
        {
            foreach (Pickable pickable in EntitiesManager.Instance.pickables)
            {
                pickable.Despawn();
            }
        }

        private void OnPickablePicked(Pickable pickable) => pickable.Despawn();
    }
}
