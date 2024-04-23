using UnityEngine;
using UnityEditor;

namespace Move.Animation
{
    /// <summary>
    ///
    /// </summary>
    public class AnimationQuantizer : AssetPostprocessor
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="root"></param>
        /// <param name="clip"></param>
        private void OnPostprocessAnimation(GameObject root, AnimationClip clip)
        {
            if (AnimationQuantizerSettings.Enabled)
            {
                Debug.Log($"Quanitizing animation clip '{clip.name}'");
                var curveBindings = AnimationUtility.GetCurveBindings(clip);
                foreach (var curveBinding in curveBindings)
                {
                    var curve = AnimationUtility.GetEditorCurve(clip, curveBinding);
                    for (int i = 0; i < curve.keys.Length; i++)
                    {
                        //probably not worth doing ALL of these but hey, let's not take any chances at this point
                        curve.keys[i].inWeight = 0;
                        curve.keys[i].outWeight = 0;
                        curve.keys[i].inTangent = 0;
                        curve.keys[i].outTangent = 0;
                        curve.keys[i].weightedMode = WeightedMode.None;
                        AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                        AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                    }
                    AnimationUtility.SetEditorCurve(clip, curveBinding, curve);
                }
            }
        }
    }

    public static class AnimationQuantizerSettings
    {
        public static bool Enabled = true;
    }
}
