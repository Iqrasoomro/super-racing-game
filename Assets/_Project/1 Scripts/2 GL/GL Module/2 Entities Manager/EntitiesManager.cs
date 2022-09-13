using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.GL.Abstract;
using ArcadianLab.SimFramework.GL.Entities;
using ArcadianLab.SimFramework.Data;

namespace ArcadianLab.SimFramework.GL
{
    public enum EntitiesManagerState
    {
        None,
        _Init,
        InitEntities,
        StartEntities,
        StopEntities,
        ClearEntities,
        _Dispose
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.Data)]
    public class EntitiesManager : SubManager
    {
        public static EntitiesManager Instance;
        private EntitiesManagerState _state;
        public static event Action<Enum> StateChanged;
        public static event Action GetEntities;
        public static event Action<Level> GetEntitiesPositions;
        public Player player;
        public Enemy enemy;
        public List<Pickable> pickables;

        public override void Init() => Instance = this;

        private void OnEnable()
        {
            GLModule.StateChanged += OnParentManager_StateUpdated;
            EntitiesSupplier.ReceiveEntities += GetEntities_Callback;
            LevelTransformSupplier.EntitiesPosition += GetEntitiesPosition_Callback;


        }
        private void OnDisable()
        {
            GLModule.StateChanged -= OnParentManager_StateUpdated;
            EntitiesSupplier.ReceiveEntities -= GetEntities_Callback;
            LevelTransformSupplier.EntitiesPosition -= GetEntitiesPosition_Callback;
        }

        public EntitiesManagerState State
        {
            get { return _state; }
            private set
            {
                if (_state == value) return;
                _state = value;
                StateChanged?.Invoke(_state);
                OnInternalStateUpdated();
                Debug.Log($"--- SystemState :{_state} ---");
            }
        }

        protected override void OnParentManager_StateUpdated(Enum type)
        {
            GLState glState = (GLState)type;
            if (State == EntitiesManagerState.None)
            {
                if (glState == GLState._Init) State = EntitiesManagerState._Init;
            }
            else if (State == EntitiesManagerState._Init || State == EntitiesManagerState.StopEntities || State == EntitiesManagerState._Dispose)
            {
                if (glState == GLState.Entities) State = EntitiesManagerState.InitEntities;
            }
            else if (State == EntitiesManagerState.InitEntities || State == EntitiesManagerState.StopEntities)
            {
                if (glState == GLState.Gameplay_Start) State = EntitiesManagerState.StartEntities;
            }
            else if (State == EntitiesManagerState.StartEntities)
            {
                if (glState == GLState.Gameplay_Stop)
                {
                    State = EntitiesManagerState.StopEntities;
                    State = EntitiesManagerState.ClearEntities;
                }
            }
            else
            {
                if (glState == GLState._Dispose) State = EntitiesManagerState._Dispose;
            }
        }

        protected override void OnInternalStateUpdated()
        {
            Debug.Log($"{gameObject.name} OnInternalStateUpdated :{_state}");
            if (State == EntitiesManagerState._Init) Init();
            else if (State == EntitiesManagerState.InitEntities) GetEntities?.Invoke();
        }

        private void GetEntities_Callback(Player player, Enemy enemy, List<Pickable> pickables)
        {
            this.player = player;
            this.enemy = enemy;
            this.pickables = pickables;
            GetEntitiesPositions?.Invoke(GLModule.Instance.userSelectedChapter.userSelectedLevel);
        }

        private void GetEntitiesPosition_Callback(LevelObject levelObject)
        {
            Assert.IsNotNull(levelObject.player);
            Assert.IsNotNull(levelObject.enemy);
            player.Position = levelObject.player.localPosition;
            enemy.Position = levelObject.enemy.localPosition;

            for (int i = 0; i < pickables.Count; i++)
            {
                if (i < levelObject.pickables.Count)
                {
                    Assert.IsNotNull(levelObject.pickables[i]);
                    pickables[i].Position = levelObject.pickables[i].transform.localPosition;
                }
                else pickables[i].Position = new Vector3(0, 0, -100);//Temp
            }
        }
    }
}
