using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace PawnBar
{
    public static class PawnSlotDrawer
    {
        #region Fields

        private static Dictionary<string, Rect> _labelRect = new Dictionary<string, Rect>();

        #endregion Fields

        #region Methods

        public static Rect CacheLabelRect( string name )
        {
            // get the width
            bool WW = Text.WordWrap;
            Text.WordWrap = false;
            Text.Font = GameFont.Tiny;
            float width = Text.CalcSize( name ).x;
            Text.Font = GameFont.Small;
            Text.WordWrap = WW;

            // create rect
            Rect labelRect = new Rect(
                ( Settings.SlotSize - width ) /2f,
                Settings.SlotSize - Settings.LabelHeight,
                width,
                Settings.LabelHeight );

            // cache and return rect
            _labelRect.Add( name, labelRect );
            return labelRect;
        }

        public static void DrawPawnInSlot( Pawn pawn, Rect slot )
        {
            // get the pawn's graphics set, and make sure it's resolved.
            PawnGraphicSet graphics = pawn.Drawer.renderer.graphics;
            if ( !graphics.AllResolved )
                graphics.ResolveAllGraphics();

            // draw base body
            GUI.color = graphics.nakedGraphic.Color;
            GUI.DrawTexture( slot, graphics.nakedGraphic.MatFront.mainTexture );

            // draw apparel
            bool drawHair = true;
            foreach ( var apparel in graphics.apparelGraphics )
            {
                if ( apparel.sourceApparel.def.apparel.LastLayer == ApparelLayer.Overhead )
                {
                    drawHair = false;
                    continue;
                }
                GUI.color = apparel.graphic.Color;
                GUI.DrawTexture( slot, apparel.graphic.MatFront.mainTexture );
            }

            // draw head, offset further drawing up
            slot.y -= Settings.SlotSize * 1/4f;
            GUI.color = graphics.headGraphic.Color;
            GUI.DrawTexture( slot, graphics.headGraphic.MatFront.mainTexture );

            // draw hair OR hat
            if ( drawHair )
            {
                GUI.color = graphics.hairGraphic.Color;
                GUI.DrawTexture( slot, graphics.hairGraphic.MatFront.mainTexture );
            }
            else
            {
                foreach ( var apparel in graphics.apparelGraphics )
                {
                    Rect slot2 = slot;
                    if ( apparel.sourceApparel.def.apparel.LastLayer == ApparelLayer.Overhead )
                    {
                        GUI.color = apparel.graphic.Color;
                        GUI.DrawTexture( slot2, apparel.graphic.MatFront.mainTexture );
                    }
                }
            }

            // draw dead, frozen overlay

            // reset color, and then we're done here
            GUI.color = Color.white;
        }

        public static void DrawSlot( this Pawn pawn, Rect slot, bool selected = false )
        {
            // background square
            Rect bgRect = slot.AtZero().ContractedBy( Settings.Inset );

            // name rect
            Rect labelRect;
            if ( !_labelRect.TryGetValue( pawn.NameStringShort, out labelRect ) )
                labelRect = CacheLabelRect( pawn.NameStringShort );

            // start drawing
            GUI.BeginGroup( slot );

            // draw background square
            GUI.DrawTexture( bgRect, TexUI.GrayBg );
            Widgets.DrawBox( bgRect );

            // draw health bar
            Rect healthBG = new Rect( bgRect.xMax, bgRect.yMin, Settings.BarWidth, bgRect.height );
            Rect healthFG = new Rect( healthBG ).ContractedBy( 1f );
            healthFG.height *= pawn.health.summaryHealth.SummaryHealthPercent;
            GUI.DrawTexture( healthBG, Resources.BarBG );
            GUI.color = pawn.health.summaryHealth.SummaryHealthPercent < .99 ? Color.red : Color.green;
            GUI.DrawTexture( healthFG, Resources.SolidWhite );
            GUI.color = Color.white;

            // if selected, draw reticule
            if ( selected )
                DrawReticule( bgRect );

            // draw pawn
            DrawPawnInSlot( pawn, slot.AtZero() );

            // draw label
            Text.Font = GameFont.Tiny;
            GUI.DrawTexture( labelRect, Resources.LabelBG );
            Widgets.Label( labelRect, pawn.NameStringShort );

            // done!
            GUI.EndGroup();
        }

        private static void DrawReticule( Rect bg )
        {
            Rect top = new Rect( bg.xMin - Settings.ReticuleThickness, bg.yMin - Settings.ReticuleThickness, bg.width + Settings.BarWidth + 2 * Settings.ReticuleThickness, Settings.ReticuleThickness );
            Rect left = new Rect( bg.xMin - Settings.ReticuleThickness, bg.yMin, Settings.ReticuleThickness, bg.height );
            Rect bottom = new Rect( top );
            Rect right = new Rect( left );
            bottom.y = bg.yMax;
            right.x = bg.xMax + Settings.BarWidth;

            GUI.DrawTexture( top, Resources.SolidWhite );
            GUI.DrawTexture( left, Resources.SolidWhite );
            GUI.DrawTexture( bottom, Resources.SolidWhite );
            GUI.DrawTexture( right, Resources.SolidWhite );
        }

        #endregion Methods
    }
}