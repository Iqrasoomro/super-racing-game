using ArcadianLab.SimFramework;

namespace ArcadianLab.SimFramework.GL.Abstract
{
    public interface IGameSystem
    {
        SystemManagerState State { get; }
        void Init();
        void StartSystem();
        void StopSystem();
        void ClearSystem();
        void Dispose();
    }
}