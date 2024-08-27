using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(AdvancedRuletileEditor))]
    [CanEditMultipleObjects]
    public class AdvancedRuletileEditor : RuleTileEditor
    {
        public Texture2D any;
        public Texture2D specific;
        
        public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
        {
            switch (neighbor)
            {
                case 3:
                    GUI.DrawTexture(rect, any);
                    return;
                case 4:
                    GUI.DrawTexture(rect, specific);
                    return;
            }
            base.RuleOnGUI(rect, position, neighbor);
        }
    }
}