using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RhythmGame
{
    /// <summary>
    /// Track Data Model Custom Inspector.
    /// </summary>
    [CustomEditor(typeof(Track))]
    public class TrackEditor : Editor
    {
        private Track track;

        private Vector2 _beatsDataScrollViewPosition;
        private bool _displayBeatsData;

        /// <summary>
        /// Assign the track on enable
        /// </summary>
        private void OnEnable()
        {
            track = (Track)target;
        }

        /// <summary>
        /// Create a button that will allow user to click to generate beats in the editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (track.beats.Count == 0)
            {
                EditorGUILayout.HelpBox("Empty Track", MessageType.Info);

                if (GUILayout.Button("Generate Random Track", EditorStyles.miniButton))
                    track.Randomize();
            }
            else
            {
                if (GUILayout.Button("Update Random Track", EditorStyles.miniButton))
                    track.Randomize();

                _displayBeatsData = EditorGUILayout.Foldout(_displayBeatsData, "Display Beats");

                if (_displayBeatsData)
                {
                    _beatsDataScrollViewPosition = EditorGUILayout.BeginScrollView(_beatsDataScrollViewPosition);

                    for (int i = 0; i < track.beats.Count; i++)
                    {
                        track.beats[i] = EditorGUILayout.IntSlider(track.beats[i], -1, Track.inputs - 1);
                    }

                    EditorGUILayout.EndScrollView();
                }
            }
            // mark the track object "dirty" so that the editor saves it on project save
            EditorUtility.SetDirty(track); 
        }
    }
}