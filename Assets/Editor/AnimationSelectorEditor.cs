using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Project.Jam;

[CustomEditor(typeof(StartAnimationPlayer))]
public class AnimationSelectorEditor : Editor
{
    private List<string> animationNames = new List<string>();
    private int selectedIndex;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        StartAnimationPlayer animationSelector = (StartAnimationPlayer)target;

        if (animationSelector.animator != null && animationSelector.animator.runtimeAnimatorController != null)
        {
            AnimationClip[] animationClips = animationSelector.animator.runtimeAnimatorController.animationClips;
            animationNames = animationClips.Select(clip => clip.name).ToList();

            if (!string.IsNullOrEmpty(animationSelector.selectedAnimation))
            {
                selectedIndex = animationNames.IndexOf(animationSelector.selectedAnimation);
            }

            selectedIndex = EditorGUILayout.Popup("Select Animation", selectedIndex, animationNames.ToArray());

            if (animationNames.Count > 0)
            {
                animationSelector.selectedAnimation = animationNames[selectedIndex];
            }
            else
            {
                animationSelector.selectedAnimation = "";
            }
        }
    }
}
