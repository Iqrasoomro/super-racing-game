using ArcadianLab.SimFramework.GL.Data;
using System;
using UnityEngine;

namespace ArcadianLab.SimFramework
{
    public interface IManager
    {
        void Instantiate();
        void Instantiate_Callback();
    }

    [DefaultExecutionOrder((int)ExecutionOrder.BeforeSystem)]
    public abstract class Manager : MonoBehaviour, IObject, IManager
    {
        abstract public void Init();
        abstract public void Instantiate();
        public void Instantiate_Callback() => Root.Instance.ManagerInstantiation_Callback();
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.Managers)]
    public abstract class SubManager : Manager
    {
        public override void Instantiate() { }
        public new void Instantiate_Callback() { }
        abstract protected void OnParentManager_StateUpdated(Enum type);
        abstract protected void OnInternalStateUpdated();
    }
}
