using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace PawnBar
{
    public class Mapcomponent_PawnBar : MapComponent
    {
        #region Fields

        private float _lastClick;
        private Pawn _selected;

        #endregion Fields

        #region Properties

        public Pawn Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        #endregion Properties

        #region Methods

        public override void MapComponentOnGUI()
        {
            // init assets.
            Resources.InitIfNeeded();

            // size calculations
            int pawnCount = Find.MapPawns.FreeColonists.Count();
            float maxWidth = Mathf.Min( pawnCount * ( Settings.TotalSize ), Screen.width * Settings.MaxScreenWidthProportion );
            int pawnsPerRow = (int)( maxWidth / ( Settings.TotalSize ) );
            float width = pawnsPerRow * ( Settings.TotalSize );

            // main rect for pawn icons,
            Rect barRect = new Rect(
                ( Screen.width - maxWidth ) / 2f,
                Settings.Margin,
                maxWidth,
                5 * ( Settings.TotalSize ) );

            // loop over pawns
            GUI.BeginGroup( barRect );
            int i = 0;
            foreach ( var pawn in Find.MapPawns.FreeColonists )
            {
                int x = i % pawnsPerRow;
                int y = i / pawnsPerRow;

                Rect slot = new Rect( x * ( Settings.TotalSize ), y * ( Settings.TotalSize ), Settings.SlotSize, Settings.SlotSize );
                pawn.DrawSlot( slot, pawn.Selected() );

                if ( Widgets.InvisibleButton( slot ) )
                {
                    // double click, move to pawn
                    if ( Time.time - _lastClick < Settings.DoubleClick )
                        Find.CameraMap.JumpTo( pawn.Position );

                    // middle button, select all
                    else if ( Event.current.button == 2 )
                    {
                        foreach ( Pawn pawn2 in Find.MapPawns.FreeColonists )
                        {
                            pawn2.Select( true );
                        }
                    }

                    // shifted, add/remove from selection
                    else if ( Event.current.shift )
                    {
                        if ( pawn.Selected() )
                            pawn.Deselect();
                        else
                            pawn.Select( true );
                    }

                    // normal click, deselect if only and selected - select if multiple or only and not selected.
                    else
                    {
                        if ( pawn.Selected() && Find.Selector.SelectedObjects.Count() > 1 )
                            pawn.Select(); // without true, this will clear everything else first.
                        else if ( !pawn.Selected() )
                            pawn.Select();
                        else if ( pawn.Selected() )
                            pawn.Deselect();
                    }

                    _lastClick = Time.time;
                }

                i++;
            }
            GUI.EndGroup();
        }

        #endregion Methods
    }
}