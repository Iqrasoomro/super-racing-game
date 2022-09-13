using System;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.NGL.Audio;

namespace ArcadianLab.SimFramework
{
    /// <summary>
    /// Views are radically bound in their role, they merely toggles on/off and the fundamental functionaly is
    /// kept in parent class to not allow deriving (view) classes perform toggling on their own that will cause
    /// erronous behavior and will have a manual operational risk
    /// </summary>
    public enum ViewState
    {
        None,
        _Init,
        ShowDefaultView,
        HideDefaultView,
        _Dispose
    }

    [Serializable]
    public abstract class View : Object_
    {
        [SerializeField] protected ViewState _state;
        [SerializeField] protected GameObject viewObject;

        private void Awake() => Show(false);

        public override void Init()
        {
            Debug.Log($"{gameObject.name} Init");
            Assert.IsNotNull(viewObject);
        }

        abstract protected void OnParentStateUpdated(Enum type);
        abstract public ViewState State { get; set; }
        protected void Show(bool value, bool playsound = false)
        {
            if(playsound) if (AudioManager.Instance) AudioManager.Instance.Play(SfxType.Tap);
            viewObject.SetActive(value);
        }

        protected virtual void OnInternalStateUpdated()
        {
            if (State == ViewState._Init) Init();
            else if (State == ViewState.ShowDefaultView) Show(true, true);
            else if (State == ViewState.HideDefaultView || State == ViewState._Dispose) Show(false, true);
        }
    }
}