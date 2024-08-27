﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMask : Image
{
        public override Material materialForRendering
        {
                get
                {
                        Material mat = new Material(base.materialForRendering);
                        mat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                        return mat;
                }
        }
}