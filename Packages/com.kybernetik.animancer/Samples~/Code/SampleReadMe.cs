// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Animancer.Samples
{
    /// <summary>[Editor-Only] A component which explains a sample scene.</summary>
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples/SampleReadMe
    [AnimancerHelpUrl(typeof(SampleReadMe))]
    [AddComponentMenu("")]
    public class SampleReadMe : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField, Multiline(10)]
        private string _Text;

        /************************************************************************************************************************/

#if UNITY_IMGUI
        /************************************************************************************************************************/

        [InitializeOnLoadMethod]
        private static void InitializeAutoSelect()
        {
            bool isInEditMode = !EditorApplication.isPlayingOrWillChangePlaymode;

            EditorApplication.playModeStateChanged += change =>
            {
                switch (change)
                {
                    case PlayModeStateChange.EnteredEditMode:
                        isInEditMode = true;
                        break;

                    default:
                        isInEditMode = false;
                        break;
                }
            };

            EditorSceneManager.activeSceneChangedInEditMode += (from, to) =>
            {
                if (!isInEditMode)
                    return;

                Object selected = Selection.activeObject;
                if (selected != null && selected is not SceneAsset)
                    return;

                foreach (GameObject gameObject in to.GetRootGameObjects())
                {
                    if (gameObject.TryGetComponent(out SampleReadMe instance))
                    {
                        EditorApplication.delayCall += () =>
                        {
                            Selection.activeObject = instance.gameObject;
                            InternalEditorUtility.SetIsInspectorExpanded(instance, true);
                        };
                        break;
                    }
                }
            };
        }

        /************************************************************************************************************************/

        /// <summary>Custom editor for <see cref="SampleReadMe"/>.</summary>
        [CustomEditor(typeof(SampleReadMe))]
        public class Editor : UnityEditor.Editor
        {
            /************************************************************************************************************************/

            private static GUIStyle _HeaderStyle;
            private static GUIStyle _BodyStyle;
            private static GUIStyle _FooterStyle;

            private static void InitializeStyles()
            {
                if (_HeaderStyle != null)
                    return;

                GUIStyle label = GUI.skin.label;

                _HeaderStyle = new(label)
                {
                    stretchWidth = false,
                };

                _HeaderStyle.fontSize *= 2;
                _HeaderStyle.normal.textColor = _HeaderStyle.hover.textColor =
                    new Color32(0x00, 0x78, 0xDA, 0xFF);

                _BodyStyle = new(label)
                {
                    richText = true,
                    wordWrap = true,
                };

                _FooterStyle = new(label)
                {
                    fontStyle = FontStyle.Italic,
                };

                _FooterStyle.fontSize = (int)(_FooterStyle.fontSize * 0.75f);
            }

            /************************************************************************************************************************/

            private static readonly GUIContent UrlContent = new("", "Click to copy this link to the clipboard");

            /// <inheritdoc/>
            public override void OnInspectorGUI()
            {
                InitializeStyles();

                GUILayout.BeginVertical();

                SampleReadMe target = (SampleReadMe)this.target;

                Scene scene = target.gameObject.scene;
                if (!scene.IsValid())
                {
                    GUILayout.Label("This component only works inside scenes.", _BodyStyle);
                    return;
                }

                string url = GetDocumentationURL(scene.path);

                // Header.

                string number = GetSampleNumber(scene);
                if (!string.IsNullOrEmpty(number))
                    GUILayout.Label(number, _FooterStyle);

                GUILayout.Label(scene.name, _HeaderStyle);

                Rect headerArea = GUILayoutUtility.GetLastRect();
                headerArea.y += headerArea.height;
                headerArea.height *= 0.05f;

                EditorGUI.DrawRect(headerArea, _HeaderStyle.normal.textColor);

                // URL.

                UrlContent.text = url;
                if (GUILayout.Button(UrlContent, _FooterStyle))
                {
                    GUIUtility.systemCopyBuffer = url;
                    Debug.Log($"Copied '{url}' to the clipboard.");
                }

                Rect urlArea = GUILayoutUtility.GetLastRect();
                EditorGUIUtility.AddCursorRect(urlArea, MouseCursor.Text);

                GUILayout.Space(_BodyStyle.fontSize);

                // Body.

                if (!string.IsNullOrEmpty(target._Text))
                {
                    GUILayout.Label(target._Text, _BodyStyle);

                    GUILayout.Space(_BodyStyle.fontSize);
                }

                // Footer.

                GUILayout.Label("Click here to open the detailed online documentation.", _FooterStyle);

                GUILayout.EndVertical();

                // Click to open URL.

                Rect area = GUILayoutUtility.GetLastRect();

                EditorGUIUtility.AddCursorRect(area, MouseCursor.Link);

                Event currentEvent = Event.current;
                if (currentEvent.type == EventType.MouseUp &&
                    area.Contains(currentEvent.mousePosition))
                    Application.OpenURL(url);

                //base.OnInspectorGUI();// Uncomment to allow editing.
            }

            /************************************************************************************************************************/

            [NonSerialized]
            private string _SampleNumber;

            private string GetSampleNumber(Scene scene)
            {
                if (_SampleNumber is not null)
                    return _SampleNumber;

                try
                {
                    string directory = Path.GetDirectoryName(scene.path);

                    string name = Path.GetFileName(directory);
                    int space = name.IndexOf(' ');

                    _SampleNumber = name[..space];

                    directory = Path.GetDirectoryName(directory);

                    name = Path.GetFileName(directory);
                    space = name.IndexOf(' ');

                    _SampleNumber = $"{name[..space]}-{_SampleNumber}";
                }
                catch (Exception exception)
                {
                    _SampleNumber = "";
                    _DocumentationURL = $"Failed to get Sample Number for {scene.path}: {exception}";
                }

                return _SampleNumber;
            }

            /************************************************************************************************************************/

            [NonSerialized]
            private string _DocumentationURL;

            private string GetDocumentationURL(string scenePath)
            {
                if (_DocumentationURL is not null)
                    return _DocumentationURL;

                try
                {
                    scenePath = Path.GetDirectoryName(scenePath);
                    string urlPath = Path.Combine(scenePath, "Documentation.URL");

                    string urlText = File.ReadAllText(urlPath);

                    const string Prefix = "URL=";

                    int start = urlText.IndexOf(Prefix) + Prefix.Length;
                    int end = urlText.IndexOf('\n', start);
                    _DocumentationURL = urlText[start..end];
                }
                catch (Exception exception)
                {
                    _DocumentationURL = $"Failed to get Documentation URL for {scenePath}: {exception}";
                }

                return _DocumentationURL;
            }

            /************************************************************************************************************************/
        }
        
        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}

#endif

