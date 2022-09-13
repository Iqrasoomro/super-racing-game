using System;
using System.Collections;
using UnityEngine;
using ArcadianLab.SimFramework.Data;

namespace ArcadianLab.SimFramework.Utils
{
    public static class OpenUrl
    {
        public static void OpenHomepage() => Application.OpenURL(Constants.Homepage);
    }
}
