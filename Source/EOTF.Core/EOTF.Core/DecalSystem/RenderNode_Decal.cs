using UnityEngine;
using Verse;

namespace EOTF.Core.DecalSystem
{
    public class PawnRenderNodeDecal : PawnRenderNode
    {

        private readonly DecalSlot _slot;

        private Graphic? _cachedGraphic;
        private string?  _cachedPath;
        private Color    _cachedColor;

        public PawnRenderNodeDecal(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
            _slot = DetermineSlot(props);
        }

        public override Graphic? GraphicFor(Pawn pawn)
        {
            var eotfProps = Props as PawnRenderNodePropertiesOmni;

            DecalProfile profile   = DecalUtil.ReadProfileFrom(pawn, _slot);
            string       path      = profile.Active ? profile.SymbolPath : GetDefaultPath(pawn);
            Color        finalColor = profile.Active ? profile.SymbolColor : (eotfProps?.Color ?? new Color(0.2f, 0.2f, 0.2f));

            if (path.NullOrEmpty()) return null;

            if (_cachedPath == path && _cachedColor == finalColor)
                return _cachedGraphic;

            _cachedPath    = path;
            _cachedColor   = finalColor;
            _cachedGraphic = GraphicDatabase.Get<Graphic_Multi>(path, ShaderDatabase.Cutout, Vector2.one, finalColor);

            return _cachedGraphic;
        }

        private static DecalSlot DetermineSlot(PawnRenderNodeProperties props)
        {
            if (props is PawnRenderNodePropertiesOmni eotfProps && eotfProps.ExplicitSlot.HasValue)
                return eotfProps.ExplicitSlot.Value;

            if (props.parentTagDef != null)
            {
                string tagName = props.parentTagDef.defName;
                if (tagName.Contains("Head") || tagName.Contains("Headgear") || tagName.Contains("Helmet"))
                    return DecalSlot.Helmet;
            }

            return DecalSlot.Armor;
        }

        private string GetDefaultPath(Pawn pawn)
        {
            if (Props is PawnRenderNodePropertiesOmni eotfProps && eotfProps.texPaths.Count > 0)
            {
                int seed = pawn.Faction?.loadID ?? pawn.thingIDNumber;
                return eotfProps.texPaths[seed % eotfProps.texPaths.Count];
            }
            return "";
        }
    }
}
