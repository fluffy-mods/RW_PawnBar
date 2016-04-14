using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PawnBar
{
    public static class Settings
    {
        public static float
            SlotSize = 70f,
            Margin = 6f,
            Inset = 16f,
            BarWidth = 4f,
            MaxScreenWidthProportion = .6f,
            LabelHeight = 16f,
            ReticuleThickness = 3f,
            DoubleClick = .3f;

        public static float TotalSize => Margin + SlotSize;
    }
}
