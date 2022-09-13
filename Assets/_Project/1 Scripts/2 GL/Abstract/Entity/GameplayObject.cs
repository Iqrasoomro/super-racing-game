using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcadianLab.SimFramework.GL.Data;
using ArcadianLab.SimFramework.GL.Systems;
using UnityEngine.Assertions;

namespace ArcadianLab.SimFramework.GL.Abstract
{
    public abstract class GameplayObject : Entity
    {
        public override void Init() => _type = EntityType.GameplayObject;
    }
}
