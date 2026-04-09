using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EOTF.Core.DecalSystem
{
    public class PawnRenderNodePropertiesOmni : PawnRenderNodeProperties
    {
        public Color Color = new Color(0.2f, 0.2f, 0.2f);

        public DecalSlot? ExplicitSlot = null;

        public new List<string> texPaths = new List<string>();

        public PawnRenderNodePropertiesOmni()
        {
            nodeClass   = typeof(PawnRenderNodeDecal);
            workerClass = typeof(PawnRenderNodeWorkerApparel);
        }
    }
}
