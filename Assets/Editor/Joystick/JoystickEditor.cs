using UnityEditor;
using UnityEngine;

namespace UI.Joystick.Editor
{
    [CustomEditor(typeof(Base.Joystick), true)]
    public class JoystickEditor : UnityEditor.Editor
    {
        private SerializedProperty handleRange;
        private SerializedProperty deadZone;
        private SerializedProperty axisOptions;
        private SerializedProperty snapX;
        private SerializedProperty snapY;
        protected SerializedProperty background;
        private SerializedProperty handle;
        private SerializedProperty directionalIndicator;
        private SerializedProperty regionalIndicator;
        private SerializedProperty skillType;
        private SerializedProperty cooldownImage;

        protected Vector2 center = new Vector2(0.5f, 0.5f);

        protected virtual void OnEnable()
        {
            handleRange = serializedObject.FindProperty("handleRange");
            deadZone = serializedObject.FindProperty("deadZone");
            axisOptions = serializedObject.FindProperty("axisOptions");
            snapX = serializedObject.FindProperty("snapX");
            snapY = serializedObject.FindProperty("snapY");
            background = serializedObject.FindProperty("background");
            handle = serializedObject.FindProperty("handle");
            directionalIndicator = serializedObject.FindProperty("directionalIndicator");
            regionalIndicator = serializedObject.FindProperty("regionalIndicator");
            skillType = serializedObject.FindProperty("skillType");
            cooldownImage = serializedObject.FindProperty("cooldownImage");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawValues();
            EditorGUILayout.Space();
            DrawComponents();

            serializedObject.ApplyModifiedProperties();

            if(handle != null)
            {
                RectTransform handleRect = (RectTransform)handle.objectReferenceValue;
                handleRect.anchorMax = center;
                handleRect.anchorMin = center;
                handleRect.pivot = center;
                handleRect.anchoredPosition = Vector2.zero;
            }
        }

        protected virtual void DrawValues()
        {
            EditorGUILayout.PropertyField(handleRange, new GUIContent("Handle Range", "The distance the visual handle can move from the center of the joystick."));
            EditorGUILayout.PropertyField(deadZone, new GUIContent("Dead Zone", "The distance away from the center input has to be before registering."));
            EditorGUILayout.PropertyField(axisOptions, new GUIContent("Axis Options", "Which axes the joystick uses."));
            EditorGUILayout.PropertyField(snapX, new GUIContent("Snap X", "Snap the horizontal input to a whole value."));
            EditorGUILayout.PropertyField(snapY, new GUIContent("Snap Y", "Snap the vertical input to a whole value."));

            if (skillType != null)
            {
                EditorGUILayout.PropertyField(directionalIndicator, new GUIContent("directionalIndicator", "directional indicator"));
                EditorGUILayout.PropertyField(regionalIndicator, new GUIContent("regionalIndicator", "regional indicator"));
                EditorGUILayout.PropertyField(skillType, new GUIContent("skillType", "Skill type"));
                EditorGUILayout.PropertyField(cooldownImage, new GUIContent("cooldownImage", "Radial cooldown"));
            }
        }

        protected virtual void DrawComponents()
        {
            EditorGUILayout.ObjectField(background, new GUIContent("Background", "The background's RectTransform component."));
            EditorGUILayout.ObjectField(handle, new GUIContent("Handle", "The handle's RectTransform component."));
        }
    }
}