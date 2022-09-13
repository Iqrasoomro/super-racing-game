using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ArcadianLab.SimFramework.Core
{
    public abstract class SingletonManagerWithHomeView : Manager
    {
        [SerializeField] protected View view;

        public override void Init()
        {
            Debug.Log($"{gameObject.name} Init");
            Assert.IsNotNull(view);
        }

        abstract protected void OnParentManager_StateUpdated(Enum type);
        abstract protected void OnInternalStateUpdated();
    }
}
