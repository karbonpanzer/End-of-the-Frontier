using Verse;
using UnityEngine;

namespace EOTF.Core.DecalSystem
{
    public sealed class CompEditDecalMarker : ThingComp 
    {
        public DecalProfile Profile = DecalProfile.Default;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref Profile.Active, "eotfDecalActive");
            Scribe_Values.Look(ref Profile.SymbolPath, "eotfDecalPath", "");
            Scribe_Values.Look(ref Profile.SymbolColor, "eotfDecalColor", Color.white);
        }
    }

    public sealed class CompPropertiesEditDecalMarker : CompProperties
    {
        public CompPropertiesEditDecalMarker() => compClass = typeof(CompEditDecalMarker);
    }
}