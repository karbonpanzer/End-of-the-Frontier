using UnityEngine;
using Verse;

namespace EOTF.Core.DecalSystem
{
    public class PawnRenderNodeDecal : PawnRenderNode
    {
        public PawnRenderNodeDecal(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) 
            : base(pawn, props, tree) { }

        public override Graphic? GraphicFor(Pawn pawn)
        {
            // Both the Helmet and Armor will share for now
            DecalProfile profile = DecalUtil.ReadProfileFrom(pawn);
            var EOTFProps = Props as PawnRenderNodePropertiesOmni;
            
            string path = profile.Active ? profile.SymbolPath : GetDefaultPath(pawn);
            Color finalColor = profile.Active ? profile.SymbolColor : (EOTFProps?.Color ?? new Color(0.2f, 0.2f, 0.2f));

            if (path.NullOrEmpty()) return null;
            
            float finalScale = Props.drawData.ScaleFor(pawn);

            return GraphicDatabase.Get<Graphic_Multi>(path, ShaderDatabase.Cutout, Vector2.one * finalScale, finalColor);
        }

        private string GetDefaultPath(Pawn pawn)
        {
            if (Props is PawnRenderNodePropertiesOmni EOTFProps && EOTFProps.texPaths.Count > 0)
            {
                int seed = pawn.Faction?.loadID ?? pawn.thingIDNumber;
                return EOTFProps.texPaths[seed % EOTFProps.texPaths.Count];
            }
            return "";
        }
    }
}