using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.GL;


public class LevelTransformSupplier : MonoBehaviour
{
    public static event Action<LevelObject> EntitiesPosition;
    [SerializeField] List<LevelObject> levelObjects;

    private void Start()
    {
        Assert.IsNotNull(levelObjects);
        Assert.AreNotEqual(0, levelObjects.Count);
        foreach (LevelObject lObj in levelObjects) lObj.gameObject.SetActive(false);
    }

    private void OnEnable() => EntitiesManager.GetEntitiesPositions += OnGetEntitiesPositions;

    private void OnDisable() => EntitiesManager.GetEntitiesPositions -= OnGetEntitiesPositions;

    void OnGetEntitiesPositions(Level level)
    {
        if (level.id >= levelObjects.Count) throw new Exception($"Level object doesn't exist in scene for id:{level.id}");
        if (levelObjects[level.id].pickables.Count != level.maxPickables) 
            throw new Exception($"Pickables count mismatch for Level:{level.id}");
        EntitiesPosition?.Invoke(levelObjects[level.id]);
    }
}
