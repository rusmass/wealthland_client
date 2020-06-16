using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonScale))]
class UIButtonScaleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 100f;
        EditorGUILayout.Space();

        ButtonScale mScale = target as ButtonScale;
        Transform tweenTarget = EditorGUILayout.ObjectField("Tween Target", mScale.tweenTarget, typeof(Transform), true) as Transform;
        if (mScale.tweenTarget != tweenTarget) mScale.tweenTarget = tweenTarget;

        DrawHover();
        DrawPressed();
    }

    private void DrawHover()
    {
        ButtonScale mScale = target as ButtonScale;
        bool showHover = EditorGUILayout.Toggle("Show Hover", mScale.showHover);
        if (mScale.showHover != showHover) mScale.showHover = showHover;

        if (mScale.showHover)
        {
            if (EditorTools.DrawHeader("Hover"))
            {
                EditorTools.BeginContents();
                GUILayout.BeginVertical();

                //EditorGUIUtility.labelWidth = 100f;

                GUILayout.Space(4f);
                Vector3 hover = EditorGUILayout.Vector3Field("Scale", mScale.hover);
                if (mScale.hover != hover) mScale.hover = hover;
                GUILayout.Space(4f);
                float hoverDuration = EditorGUILayout.FloatField("Duration", mScale.hoverDuration);
                if (mScale.hoverDuration != hoverDuration) mScale.hoverDuration = hoverDuration;
                GUILayout.Space(4f);

                GUILayout.EndVertical();
                EditorTools.EndContents();
            }
        }
    }

    private void DrawPressed()
    {
        //EditorGUIUtility.labelWidth = 120f;
        ButtonScale mScale = target as ButtonScale;
        bool showPressed = EditorGUILayout.Toggle("Show Pressed", mScale.showPressed);
        if (mScale.showPressed != showPressed) mScale.showPressed = showPressed;

        if (mScale.showPressed)
        {
            if (EditorTools.DrawHeader("Pressed"))
            {
                EditorTools.BeginContents();
                GUILayout.BeginVertical();

                //EditorGUIUtility.labelWidth = 100f;

                GUILayout.Space(4f);
                Vector3 pressed = EditorGUILayout.Vector3Field("Scale", mScale.pressed);
                if (mScale.pressed != pressed) mScale.pressed = pressed;
                GUILayout.Space(4f);
                float pressedDuration = EditorGUILayout.FloatField("Duration", mScale.pressedDuration);
                if (mScale.pressedDuration != pressedDuration) mScale.pressedDuration = pressedDuration;
                GUILayout.Space(4f);

                GUILayout.EndVertical();
                EditorTools.EndContents();
            }
        }
    }
}