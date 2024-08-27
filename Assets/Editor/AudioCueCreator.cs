using UnityEditor;
using UnityEngine;

public class AudioCueCreator : Editor
{
    [MenuItem("Assets/Create/Audio/AudioCue")]
    public static void CreateAudioCue()
    {
        AudioCue audioCue = ScriptableObject.CreateInstance<AudioCue>();
        audioCue.OnCreate(Selection.objects); // Initialize with selected audio clips
        ProjectWindowUtil.CreateAsset(audioCue, "New Audio Cue.asset");
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = audioCue;
    }
}

[CustomPropertyDrawer (typeof(NamedArrayAttribute))]public class NamedArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        try {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            EditorGUI.ObjectField(rect, property, new GUIContent(((NamedArrayAttribute)attribute).names[pos]));
        } catch {
            EditorGUI.ObjectField(rect, property, label);
        }
    }
}