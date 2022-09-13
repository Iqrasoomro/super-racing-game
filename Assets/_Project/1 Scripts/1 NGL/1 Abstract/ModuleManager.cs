

namespace ArcadianLab.SimFramework
{
    /// <summary>
    /// Module manager: Manager with sub-managers and no views
    /// It works directly under GameManager and is responsible for it's entire module
    /// </summary>
    public abstract class ModuleManager : Manager
    {
        private void OnEnable() => GameManager.GameStateChanged += OnGameManager_StateChanged;
        private void OnDisable() => GameManager.GameStateChanged -= OnGameManager_StateChanged;
        abstract protected void OnGameManager_StateChanged(GameManagerState state);
        abstract protected void OnInternalStateUpdated();
    }
}
