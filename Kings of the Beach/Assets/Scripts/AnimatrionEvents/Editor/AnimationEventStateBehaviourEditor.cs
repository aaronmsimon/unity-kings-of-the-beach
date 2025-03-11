using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace adammyhre.ImprovedUnityAnimationEvents {
    /// <summary>
    /// Custom editor for the AnimationEventStateBehaviour class, providing a GUI for previewing animation states
    /// and handling animation events within the Unity editor. Enables users to preview animations and manage
    /// animation events directly in the editor.
    /// </summary>
    [CustomEditor(typeof(AnimationEventStateBehaviour))]
    public class AnimationEventStateBehaviourEditor : Editor {
        Motion previewClip;
        float previewTime;
        bool isPreviewing;

        PlayableGraph playableGraph;
        AnimationMixerPlayable mixer;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            AnimationEventStateBehaviour stateBehaviour = (AnimationEventStateBehaviour) target;

            if (Validate(stateBehaviour, out string errorMessage)) {
                GUILayout.Space(10);

                if (isPreviewing) {
                    if (GUILayout.Button("Stop Preview")) {
                        // EnforceTPose();
                        isPreviewing = false;
                        AnimationMode.StopAnimationMode();
                        playableGraph.Destroy();
                    } else {
                        PreviewAnimationClip(stateBehaviour);
                    }
                } else if (GUILayout.Button("Preview")) {
                    isPreviewing = true;
                    AnimationMode.StartAnimationMode();
                }

                GUILayout.Label($"Previewing at {previewTime:F2}s", EditorStyles.helpBox);
            } else {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
            }
        }

        void PreviewAnimationClip(AnimationEventStateBehaviour stateBehaviour) {
            AnimatorController animatorController = GetValidAnimatorController(out string errorMessage);
            if (animatorController == null) return;

            ChildAnimatorState matchingState = animatorController.layers
                .Select(layer => FindMatchingState(layer.stateMachine, stateBehaviour))
                .FirstOrDefault(state => state.state != null);

            if (matchingState.state == null) return;

            Motion motion = matchingState.state.motion;

            // If it's a simple AnimationClip, sample it directly
            if (motion is AnimationClip clip) {
                previewTime = stateBehaviour.triggerTime * clip.length;
                AnimationMode.SampleAnimationClip(Selection.activeGameObject, clip, previewTime);
            }
        }
        
        float CalculateWeightForChild(BlendTree blendTree, ChildMotion child, float targetWeight) {
            float weight = 0f;

            if (blendTree.blendType == BlendTreeType.Simple1D) {
                // Find the neighbors around the target weight
                ChildMotion? lowerNeighbor = null;
                ChildMotion? upperNeighbor = null;

                foreach (var motion in blendTree.children) {
                    if (motion.threshold <= targetWeight && (lowerNeighbor == null || motion.threshold > lowerNeighbor.Value.threshold)) {
                        lowerNeighbor = motion;
                    }

                    if (motion.threshold >= targetWeight && (upperNeighbor == null || motion.threshold < upperNeighbor.Value.threshold)) {
                        upperNeighbor = motion;
                    }
                }

                if (lowerNeighbor.HasValue && upperNeighbor.HasValue) {
                    if (Mathf.Approximately(child.threshold, lowerNeighbor.Value.threshold)) {
                        weight = 1.0f - Mathf.InverseLerp(lowerNeighbor.Value.threshold, upperNeighbor.Value.threshold, targetWeight);
                    } else if (Mathf.Approximately(child.threshold, upperNeighbor.Value.threshold)) {
                        weight = Mathf.InverseLerp(lowerNeighbor.Value.threshold, upperNeighbor.Value.threshold, targetWeight);
                    }
                } else {
                    // Handle edge cases where there is no valid interpolation range
                    weight = Mathf.Approximately(targetWeight, child.threshold) ? 1f : 0f;
                }
            } else if (blendTree.blendType == BlendTreeType.FreeformCartesian2D || blendTree.blendType == BlendTreeType.FreeformDirectional2D) {
                Vector2 targetPos = new(
                    GetBlendParameterValue(blendTree, blendTree.blendParameter),
                    GetBlendParameterValue(blendTree, blendTree.blendParameterY)
                );
                float distance = Vector2.Distance(targetPos, child.position);
                weight = Mathf.Clamp01(1.0f / (distance + 0.001f));
            }

            return weight;
        }


        float GetBlendParameterValue(BlendTree blendTree, string parameterName) {
            var methodInfo = typeof(BlendTree).GetMethod("GetInputBlendValue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null) {
                Debug.LogError("Failed to find GetInputBlendValue method via reflection.");
                return 0f;
            }

            return (float) methodInfo.Invoke(blendTree, new object[] { parameterName });
        }

        ChildAnimatorState FindMatchingState(AnimatorStateMachine stateMachine, AnimationEventStateBehaviour stateBehaviour) {
            foreach (var state in stateMachine.states) {
                if (state.state.behaviours.Contains(stateBehaviour)) {
                    return state;
                }
            }

            foreach (var subStateMachine in stateMachine.stateMachines) {
                var matchingState = FindMatchingState(subStateMachine.stateMachine, stateBehaviour);
                if (matchingState.state != null) {
                    return matchingState;
                }
            }

            return default;
        }

        bool Validate(AnimationEventStateBehaviour stateBehaviour, out string errorMessage) {
            AnimatorController animatorController = GetValidAnimatorController(out errorMessage);
            if (animatorController == null) return false;

            ChildAnimatorState matchingState = animatorController.layers
                .Select(layer => FindMatchingState(layer.stateMachine, stateBehaviour))
                .FirstOrDefault(state => state.state != null);

            previewClip = GetAnimationClipFromMotion(matchingState.state?.motion);
            if (previewClip == null) {
                errorMessage = "No valid AnimationClip found for the current state.";
                return false;
            }

            return true;
        }

        AnimationClip GetAnimationClipFromMotion(Motion motion) {
            if (motion is AnimationClip clip) {
                return clip;
            }

            if (motion is BlendTree blendTree) {
                return blendTree.children
                    .Select(child => GetAnimationClipFromMotion(child.motion))
                    .FirstOrDefault(childClip => childClip != null);
            }

            return null;
        }

        AnimatorController GetValidAnimatorController(out string errorMessage) {
            errorMessage = string.Empty;

            GameObject targetGameObject = Selection.activeGameObject;
            if (targetGameObject == null) {
                errorMessage = "Please select a GameObject with an Animator to preview.";
                return null;
            }

            Animator animator = targetGameObject.GetComponent<Animator>();
            if (animator == null) {
                errorMessage = "The selected GameObject does not have an Animator component.";
                return null;
            }

            AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null) {
                errorMessage = "The selected Animator does not have a valid AnimatorController.";
                return null;
            }

            return animatorController;
        }

        [MenuItem("GameObject/Enforce T-Pose", false, 0)]
        static void EnforceTPose() {
            GameObject selected = Selection.activeGameObject;
            if (!selected || !selected.TryGetComponent(out Animator animator) || !animator.avatar) return;

            SkeletonBone[] skeletonBones = animator.avatar.humanDescription.skeleton;

            foreach (HumanBodyBones hbb in Enum.GetValues(typeof(HumanBodyBones))) {
                if (hbb == HumanBodyBones.LastBone) continue;

                Transform boneTransform = animator.GetBoneTransform(hbb);
                if (!boneTransform) continue;

                SkeletonBone skeletonBone = skeletonBones.FirstOrDefault(sb => sb.name == boneTransform.name);
                if (skeletonBone.name == null) continue;

                if (hbb == HumanBodyBones.Hips) boneTransform.localPosition = skeletonBone.position;
                boneTransform.localRotation = skeletonBone.rotation;
            }
        }
    }
}
