using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Data;

[Serializable]
public class LevelObject : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public List<Transform> pickables;

    private void Start()
    {
        Assert.IsNotNull(player);
        Assert.IsNotNull(enemy);
        Assert.IsNotNull(pickables);
        Assert.AreNotEqual(0, pickables.Count);
    }
}
