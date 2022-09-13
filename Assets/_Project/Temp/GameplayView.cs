using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.NGL.StdMenu;
using ArcadianLab.SimFramework.NGL.Utils;

public class GameplayView : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnEnable()
    {
        /*Chapter.LevelCompleted += delegate { 
            if(NGLModule.Instance.standardMenu.State != StandardMenuState.ChapterSelection)
                NGLModule.Instance.standardMenu.State = StandardMenuState.LevelSelection; };
        Chapter.LevelRecompleted += delegate { NGLModule.Instance.standardMenu.State = StandardMenuState.LevelSelection;};
        GameData.ChapterCompleted += delegate { NGLModule.Instance.standardMenu.State = StandardMenuState.ChapterSelection; };*/
    }

    private void OnDisable()
    {
        /*Chapter.LevelCompleted -= delegate { NGLModule.Instance.standardMenu.State = StandardMenuState.LevelSelection; };
        Chapter.LevelRecompleted -= delegate { NGLModule.Instance.standardMenu.State = StandardMenuState.LevelSelection; };
        GameData.ChapterCompleted -= delegate { NGLModule.Instance.standardMenu.State = StandardMenuState.ChapterSelection; };*/
    }


    private void OnGUI()
    {
        /*GUI.skin.label.fontSize = 20;
        GUI.skin.button.fontSize = 20;
        if (NGLModule.Instance.standardMenu.State != StandardMenuState._Dispose) return;
        Chapter ch = DataManager.Instance.data.gameData.userSelectedChapter;
        if(ch != null && ch.userSelectedLevel != null)
        {
            GUILayout.Label($"Chapter = #{ch.id + 1} {ch.name}");
            GUILayout.Label($"Level = #{ch.userSelectedLevel.id + 1}");
            GUILayout.Label($"Pickables = #{ch.userSelectedLevel.maxPickables}");
            if (GUILayout.Button("Complete level"))
            {
                DataManager.Instance.data.gameData.userSelectedChapter.RequestLevelComplete(ch.userSelectedLevel);
                DataManager.Instance.Save();
            }
        }
        
        if (GUILayout.Button("Back")) NGLModule.Instance.standardMenu.State = StandardMenuState.LevelSelection;*/
    }
}
