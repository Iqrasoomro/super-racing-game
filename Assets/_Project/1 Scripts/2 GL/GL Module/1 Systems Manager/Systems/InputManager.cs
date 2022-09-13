using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcadianLab.SimFramework.GL.Systems;
using ArcadianLab.SimFramework.GL.Abstract;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework.GL.Systems
{
    public class InputManager : GameSystem
    {
        public static event Action<Vector3> InputReceived;
        private Vector3 _direction;

        public override void Init()
        {

        }

        public override void ClearSystem() => _direction = Vector3.zero;

        void Update()
        {
            if (State != SystemManagerState.Start) return;
            if (!Input.anyKey) return;

            _direction = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) _direction += Vector3.forward;
            else if (Input.GetKey(KeyCode.S)) _direction += Vector3.back;
            if (Input.GetKey(KeyCode.A)) _direction += Vector3.left;
            else if (Input.GetKey(KeyCode.D)) _direction += Vector3.right;

            if (_direction != Vector3.zero) InputReceived?.Invoke(_direction);
        }
    }
}
