using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcadianLab.SimFramework.GL.Abstract;
using System;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework.GL.Entities
{
    public class Enemy : Humanoid
    {
        void Update()
        {
            if (!IsActive) return;
            if (!EntitiesManager.Instance.player) return;
            transform.position = Vector3.MoveTowards(transform.position, EntitiesManager.Instance.player.transform.position, 2.5f * Time.deltaTime);
            transform.LookAt(EntitiesManager.Instance.player.transform);
        }
    }
}
