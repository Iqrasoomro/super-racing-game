using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcadianLab.SimFramework.GL.Data;

namespace ArcadianLab.SimFramework.GL.Abstract
{
    public interface IEntity
    {
        bool IsActive { get; set; }
        EntityType Type { get; }
        Vector3 Position { get; set; }
        public abstract void Init();
        public abstract void Spawn();
        public abstract void Despawn();
        public abstract void Dispose();
    }
}
