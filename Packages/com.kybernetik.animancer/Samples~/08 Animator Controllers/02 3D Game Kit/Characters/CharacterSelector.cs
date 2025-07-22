// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System.Text;
using UnityEngine;

#if UNITY_UGUI
using UnityEngine.UI;
#endif

namespace Animancer.Samples.AnimatorControllers.GameKit
{
    /// <summary>A simple system for selecting characters.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/3d-game-kit">
    /// 3D Game Kit</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers.GameKit/CharacterSelector
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Game Kit - Character Selector")]
    [AnimancerHelpUrl(typeof(CharacterSelector))]
    public class CharacterSelector : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_UGUI
        /************************************************************************************************************************/

        [SerializeField] private Text _Text;
        [SerializeField] private GameObject[] _Characters;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SelectCharacter(0);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            if (SampleInput.Number1Up)
                SelectCharacter(0);
            else if (SampleInput.Number2Up)
                SelectCharacter(1);
        }

        /************************************************************************************************************************/

        private void SelectCharacter(int index)
        {
            StringBuilder text = StringBuilderPool.Instance.Acquire();

            for (int i = 0; i < _Characters.Length; i++)
            {
                // Activate the target and deactivate everyone else.
                bool active = i == index;
                _Characters[i].SetActive(active);

                // Build all their names into a string explaining which keys they correspond to.

                if (i > 0)
                    text.AppendLine();

                if (active)// Use Rich Text Tags to make the active character Bold.
                    text.Append("<b>");

                text.Append(1 + i)
                    .Append(" = ")
                    .Append(_Characters[i].name);

                if (active)
                    text.Append("</b>");
            }

            _Text.text = text.ReleaseToString();
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingUnityUIModuleError(this);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
