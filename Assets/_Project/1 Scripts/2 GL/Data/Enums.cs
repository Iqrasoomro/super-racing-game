using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadianLab.SimFramework.GL.Data
{
    public enum ExecutionOrder
    {
        BeforeSystem = 100,
        System = 200,
        AfterSystem = 200,
    }

    public enum LevelConclusionType
    {
        Completed,
        Failed
    }

    public enum EntityType
    {
        None,
        Humanoid,
        GameplayObject
    }

    public enum HumanoidType
    {
        None,
        Player,
        Enemy
    }

    public enum GameplayObjectType
    {
        None,
        Pickable,
    }
}
