using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.GL;
using ArcadianLab.SimFramework.GL.Entities;


public class EntitiesSupplier : MonoBehaviour
{
    public static event Action<Player, Enemy, List<Pickable>> ReceiveEntities;
    [SerializeField] Player player;
    [SerializeField] Enemy enemy;
    [SerializeField] List<Pickable> pickables;

    private void Start()
    {
        Assert.IsNotNull(player);
        Assert.IsNotNull(enemy);
        Assert.IsNotNull(pickables);
        Assert.AreNotEqual(0, pickables.Count);
    }

    private void OnEnable() => EntitiesManager.GetEntities += OnGetEntities;

    private void OnDisable() => EntitiesManager.GetEntities -= OnGetEntities;


    private void OnGetEntities()
    {
        if (player != null && enemy != null && pickables != null)
        {
            ReceiveEntities?.Invoke(player, enemy, pickables);
        }
    }
}
