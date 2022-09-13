using UnityEngine.SceneManagement;

namespace ArcadianLab.SimFramework.NGL.Utils
{
    public class SceneManager_ : Object_
    {
        public override void Init() { }

        public void LoadScene(int chapterSceneIndex = 0)
        {
            if (SceneManager.GetActiveScene().buildIndex == chapterSceneIndex) return;
            SceneManager.LoadScene(chapterSceneIndex);
        }

        public string GetCurrentScene() { return SceneManager.GetActiveScene().name; }
    } 
}
