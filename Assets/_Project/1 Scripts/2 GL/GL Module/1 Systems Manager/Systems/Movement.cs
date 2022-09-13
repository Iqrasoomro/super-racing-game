using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.GL.Abstract;
using ArcadianLab.SimFramework.GL.Systems;
using Cube.MiniGame.Data;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework.GL.Systems
{
    public class Movement : GameSystem
    {
        private const float speed = 5f;

        private void OnEnable()
        {
            SystemsManager.StateChanged += OnParentStateUpdated;
            InputManager.InputReceived += Move;
        }

        private void OnDisable()
        {
            SystemsManager.StateChanged -= OnParentStateUpdated;
            InputManager.InputReceived -= Move;
        }

        public void Move(Vector3 direction)
        {
            if (State != SystemManagerState.Start) return;
            direction = Vector3.Normalize(direction);
            EntitiesManager.Instance.player.transform.position += direction * Time.deltaTime * speed;
        }
    }
}
