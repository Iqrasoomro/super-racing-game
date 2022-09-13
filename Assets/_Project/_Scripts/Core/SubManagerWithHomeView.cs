using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ArcadianLab.SimFramework.Core
{
    public abstract class SubManagerWithHomeView : SubManager
    {
        [SerializeField] protected View view;

        public override void Init()
        {
            Debug.Log($"{gameObject.name} Init");
            Assert.IsNotNull(view);
        }
    }
}
