using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using RimWorld;
using UnityEngine;

namespace PawnBar
{
    public static class Resources
    {
        public static Texture2D SlotBG;
        public static Texture2D LabelBG;
        public static Texture2D BarBG;
        public static Texture2D SolidWhite;

        private static bool _initialized;

        public static void InitIfNeeded()
        {
            if ( _initialized )
                return;

            SlotBG = TexUI.TextBGBlack;
            LabelBG = SolidColorMaterials.NewSolidColorTexture( new Color( 0f, 0f, 0f, .4f ) );
            BarBG = SolidColorMaterials.NewSolidColorTexture( new Color( .3f, .3f, .3f, 1f ) );
            SolidWhite = SolidColorMaterials.NewSolidColorTexture( new Color( 1f, 1f, 1f, 1f ) );

            _initialized = true;
        }
    }

}
