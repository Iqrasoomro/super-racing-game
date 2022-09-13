using UnityEngine;
using ArcadianLab.SimFramework.Data;

namespace ArcadianLab.SimFramework.Core
{
    /// <summary>
    /// Smallest unit is termed as Object_, it's encouraged to keep them as Objects_
    /// instead of deriving directly from Monobehaviors to utilize the Init() call at will
    /// </summary>
    public interface IObject
    {
        void Init();
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.Managers)]
    public abstract class Object_ : MonoBehaviour, IObject
    {
        abstract public void Init();
    }
}
