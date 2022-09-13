using ArcadianLab.SimFramework;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.GL;
using ArcadianLab.SimFramework.GL.Abstract;
using ArcadianLab.SimFramework.GL.Data;
using ArcadianLab.SimFramework.GL.Entities;
using System;

namespace ArcadianLab.SimFramework.GL.Systems
{
    public enum GameStateHandler_State
    {
        None,
        Idle,
        Gameplay,
        Gameover
    }

    public class GameStateHandler : GameSystem
    {
        //private DataManager _data;
        public static event Action<LevelConclusionType> LevelConclude;
        private Level level;
        private int pickables, maxPickables;

        public override void Init()
        {
            SystemsManager.StateChanged += OnParentStateUpdated;
            Player.PickablePicked += OnPickablePicked;
            Player.PlayerDie += OnPlayerDie;
        }
        public override void Dispose()
        {
            SystemsManager.StateChanged -= OnParentStateUpdated;
            Player.PickablePicked -= OnPickablePicked;
            Player.PlayerDie -= OnPlayerDie;
        }

        public override void StartSystem()
        {
            level = GLModule.Instance.userSelectedChapter.userSelectedLevel;
            pickables = 0;
            maxPickables = level.maxPickables;
        }

        public override void StopSystem()
        {
            
        }

        public override void ClearSystem()
        {
            level = null;
        }

        private void OnPickablePicked(Pickable pickable)
        {
            if (State != SystemManagerState.Start) return;
            if (++pickables >= maxPickables) LevelConclude?.Invoke(LevelConclusionType.Completed);

            /*if (type == BlockType.Coin) AddScore();
            else if (type == BlockType.Hurdle) LevelFailed();*/
        }

        private void OnPlayerDie()
        {
            if (State != SystemManagerState.Start) return;
            LevelConclude?.Invoke(LevelConclusionType.Failed);
        }
    }
}
