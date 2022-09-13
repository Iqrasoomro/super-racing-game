using UnityEngine;
using ArcadianLab.SimFramework.Data;

namespace ArcadianLab.SimFramework.NGL.Utils
{
    public static class OpenUrl
    {
        public static void OpenHomepage() => Application.OpenURL(Constants.Homepage);
    }
}
