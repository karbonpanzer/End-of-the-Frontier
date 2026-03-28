using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace EOTF.Core.DecalSystem
{
    [StaticConstructorOnStartup]
    public static class DecalBootstrap
    {
        static DecalBootstrap()
        {
            try
            {
                new Harmony("EOTF.Decals").PatchAll();
                Log.Message("[EOTF] Decal System loaded successfully.");
            }
            catch (System.Exception e)
            {
                Log.Error("[EOTF] Decal System failed to load:\n" + e);
            }
        }
    }

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
    public static class PatchPawnGetGizmosDecals
    {
        public static void Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
        {
            if (__instance.Faction != Faction.OfPlayerSilentFail || 
                !DecalUtil.IsHumanlikePawn(__instance) || 
                !DecalUtil.PawnHasAnyDecalApparel(__instance)) return;
        
            __result = __result.Concat(new[] { CreateDecalGizmo(__instance) });
        }

        private static Gizmo CreateDecalGizmo(Pawn pawn)
        {
            return new Command_Action
            {
                defaultLabel = "EOTF_StyleDecalsGizmo".Translate(pawn.LabelCap),
                defaultDesc = "EOTF_StyleDecalsDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/CustomizeDecal"),
                action = () => Find.WindowStack.Add(new DialogEditDecals(pawn))
            };
        }
    }
}