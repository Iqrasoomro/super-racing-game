using UnityEngine.UI;

namespace ArcadianLab.SimFramework.NGL.Utils
{
    public static class ScrollRect_
    {
        public static void ScrollToTop(this ScrollRect scrollRect) => scrollRect.horizontalNormalizedPosition = 0;

        public static void ScrollToBottom(this ScrollRect scrollRect) => scrollRect.horizontalNormalizedPosition = 1;

        public static void ScrollToPercent(this ScrollRect scrollRect, float value)
        {
            if (value < 0) value = 0;
            else if (value > 1) value = 1;
            else
            {
                if (value <= 0.15f) ScrollToTop(scrollRect);
                else if (value >= 0.85f) ScrollToBottom(scrollRect);

                else scrollRect.horizontalNormalizedPosition = value; //[0 - 1]
            }
        }
    }
}
