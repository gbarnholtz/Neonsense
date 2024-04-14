﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace z3y.Shaders
{
    [System.Serializable]
    public class ShaderSettings
    {
        [SerializeField] public string shaderName;

        public MaterialType materialType = MaterialType.Lit;
        public DefineType bicubicLightmap = DefineType.LocalKeyword;
        public DefineType gsaa = DefineType.LocalKeyword;
        public DefineType anisotropy = DefineType.Disabled;
        // fragment normal space option?
        public DefineType bakeryMonoSH = DefineType.LocalKeyword;
        public DefineType lightmappedSpecular = DefineType.LocalKeyword;

        // public Shader additionalPass;
        public bool grabPass = false;
        public bool alphaToCoverage = true;
        public string customEditorGUI;

        public enum DefineType
        {
            LocalKeyword,
            GlobalKeyword,
            Enabled,
            Disabled
        }

        [System.Flags]
        public enum ShaderPass
        {
            ForwardBase,
            ForwardAdd,
            ShadowCaster,
            Meta
        }

        public enum MaterialType
        {
            Lit,
            Unlit,
        }

        public const string MonoShKeyword = "BAKERY_MONOSH";
        public const string BicubicLightmapKeyword = "BICUBIC_LIGHTMAP";
        public const string GsaaKeyword = "_GEOMETRICSPECULAR_AA";
        public const string AnisotropyKeyword = "_ANISOTROPY";
        public const string LightmappedSpecular = "_LIGHTMAPPED_SPECULAR";


    }
}