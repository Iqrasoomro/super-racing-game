using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using ArcadianLab.SimFramework.Core;

namespace ArcadianLab.SimFramework.Utils
{
    // Scene names and SceneType entries should match in naming
    public enum SceneType { _Main, Garage }

    public class SceneManager_ : Object_
    {
        [SerializeField] private SceneType[] _scenes;
        private SceneType currentSceneType;

        public override void Init()
        {
            Assert.IsNotNull(_scenes);
            Debug.Log($"{gameObject.name}.Init() : Scenes Count {_scenes.Length}");
            #region Scene Count Checked with Enum Count
            #if UNITY_EDITOR
            int sceneArrayCount = Enum.GetValues(typeof(SceneType)).Length - 1;
            int editorBuildScenesCount = UnityEditor.EditorBuildSettings.scenes.Length - 1;
            Assert.AreEqual(sceneArrayCount, editorBuildScenesCount);
            #endif
            #endregion Scene Count Checked with Enum Count
        }

        public void LoadScene(SceneType type)
        {
            if ((int)type >= _scenes.Length) 
                throw new Exception($"SceneManager_.LoadScene() index:{(int)type} > length:{_scenes.Length}");
            else if (type == currentSceneType)
                throw new Exception($"SceneManager_.LoadScene() Reloading same scene : {type}");
            Debug.Log($"SceneManager_: Load Scene {type}");
            SceneManager.LoadScene(_scenes[(int)type].ToString());
            currentSceneType = type;
        }

        public string GetCurrentScene() { return SceneManager.GetActiveScene().name; }
    } 
}
