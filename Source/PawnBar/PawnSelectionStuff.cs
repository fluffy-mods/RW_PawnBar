using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using RimWorld;

namespace PawnBar
{
    public static class PawnSelectionStuff
    {
        public static bool Selected( this Pawn pawn )
        {
            if ( Find.Selector.IsSelected( pawn ) )
                return true;
            return false;
        }

        public static void Select( this Pawn pawn, bool add = false )
        {
            if ( !add || Find.Selector.SelectedObjectsListForReading.Any( obj => !( obj is Pawn ) ) )
                Find.Selector.ClearSelection();

            Find.Selector.Select( pawn );
        }

        public static void Deselect( this Pawn pawn )
        {
            Find.Selector.Deselect( pawn );
        }
    }
}
