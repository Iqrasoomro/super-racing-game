using System;

namespace ArcadianLab.SimFramework.Core
{
    public class RootView : View
    {
        public override void Init()
        {
            base.Init();
        }

        private void OnEnable() => Root.StateChanged += OnParentStateUpdated;
        private void OnDisable() => Root.StateChanged -= OnParentStateUpdated;

        public override ViewState State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                _state = value;
                OnInternalStateUpdated();
            }
        }

        protected override void OnParentStateUpdated(Enum type)
        {
            RootState rootState = (RootState)type;
            if (State == ViewState.None && rootState == RootState._Init) State = ViewState._Init;
            else if (State == ViewState._Init && rootState == RootState.Loading) State = ViewState.ShowDefaultView;
            else
            {
                if (State == ViewState.ShowDefaultView)
                {
                    if (rootState == RootState.LoadingComplete || rootState == RootState._Dispose) State = ViewState._Dispose;
                }
            }
        }
    }
}
