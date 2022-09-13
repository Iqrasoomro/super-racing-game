using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcadianLab.SimFramework.GL.Abstract;

namespace ArcadianLab.SimFramework.GL.Entities
{
    public class Player : Humanoid
    {
        public static event Action<Pickable> PickablePicked;
        public static event Action PlayerDie;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsActive) return;
            if (other.TryGetComponent(out Pickable pickable))
            {
                Debug.Log("Pickable");
                PickablePicked?.Invoke(pickable);
            }
            else if (other.TryGetComponent(out Enemy enemy))
            {
                Debug.Log("Enemy");
                PlayerDie?.Invoke();
            }
        }
    }
}
