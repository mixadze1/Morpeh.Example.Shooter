// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_UGUI
using UnityEngine.UI;
#endif

namespace Animancer.Samples.FineControl
{
    /// <summary>Tracks user progress through a tutorial and displays appropriate instructions.</summary>
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fine-control/live-inspector">
    /// Live Inspector</see>
    /// </remarks>
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.FineControl/LiveInspectorTutorial
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Fine Control - Live Inspector Tutorial")]
    [AnimancerHelpUrl(typeof(LiveInspectorTutorial))]
    public class LiveInspectorTutorial : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_UGUI
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private Text _Text;

        /************************************************************************************************************************/
#if !UNITY_EDITOR
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Text.text = "This Sample only works in the Unity Editor";
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        private class TutorialStep
        {
            /************************************************************************************************************************/

            /// <summary>Instructions to display while waiting for the user to complete this step.</summary>
            public readonly string Instructions;

            /// <summary>Has the user completed this step?</summary>
            public readonly Func<bool> IsComplete;

            /************************************************************************************************************************/

            public TutorialStep(string instructions, Func<bool> isComplete)
            {
                Instructions = instructions;
                IsComplete = isComplete;
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/

        private readonly List<TutorialStep> Steps = new();

        private int _CurrentStep;
        private bool _WasShowingAddAnimationField;

        private const string
            AutoNormalizeWeightsFunction = "Display Options/Auto Normalize Weights",
            ShowAddAnimationFieldFunction = "Display Options/Show 'Add Animation' Field",
            ShowAddAnimationFieldPref = "Animancer/Inspector/" + ShowAddAnimationFieldFunction,
            IdleName = "Humanoid-Idle",
            WalkName = "Humanoid-WalkForwards",
            DragAndDropAnimations =
                "\n\nYou can also Drag and Drop animations from the Project window into the preview area to add them.";

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _WasShowingAddAnimationField = UnityEditor.EditorPrefs.GetBool(ShowAddAnimationFieldPref);

            // Select AnimancerComponent.
            Steps.Add(new(
                $"Select the {_Animancer.name} object in the Hierarchy",
                () => UnityEditor.Selection.activeGameObject == _Animancer.gameObject));

            // Initialize graph.
            Steps.Add(new(
                $"Right Click on the header of the {nameof(AnimancerComponent)} in the Inspector" +
                $" and select the 'Initialize Graph' function from the context menu.",
                () => _Animancer.IsGraphInitialized));

            // Show 'Add Animation' field.
            Steps.Add(new(
                $"Note how the character is now in a hunched over pose with their legs under the ground." +
                $" That's the default pose for a Humanoid Rig when it has no animations so let's add some." +
                $"\n" +
                $"\nMake sure the preview area down the bottom of the Inspector is expanded." +
                $"\n" +
                $"\nRight Click on the word 'States' in the preview area" +
                $" and select the '{ShowAddAnimationFieldFunction}' function from the context menu.",
                () => UnityEditor.EditorPrefs.GetBool(ShowAddAnimationFieldPref)));

            // Add idle animation.
            Steps.Add(new(
                $"Use the 'Add Animation' field at the top of the preview area" +
                $" to select the '{IdleName}' animation." +
                DragAndDropAnimations,
                () => IsCurrentState(IdleName)));

            // Add walk animation.
            Steps.Add(new(
                $"Use the 'Add Animation' field at the top of the preview area" +
                $" to select the '{WalkName}' animation." +
                DragAndDropAnimations,
                () => IsCurrentState(WalkName)));

            // Play idle animation.
            Steps.Add(new(
                $"Ctrl + Left Click on the '{IdleName}' animation to play it.",
                () => IsCurrentState(IdleName)));

            // Play walk animation as well.
            Steps.Add(new(
                $"With the '{IdleName}' animation still playing," +
                $" Left Click on the '{WalkName}' animation to expand its details." +
                $"\n" +
                $"\nClick the Play button in that state's details.",
                () => IsCurrentState(IdleName)
                && TryGetState(WalkName, out AnimancerState walk)
                && walk.Weight == 0
                && walk.IsPlaying));

            // Set weight.
            Steps.Add(new(
                $"Note how the preview now shows both animations playing," +
                $" but you can still only see the '{IdleName}' pose in the Game window." +
                $" That's because interacting directly with the details of a state only affects those specific details" +
                $" and the Weight of '{WalkName}' is still 0." +
                $"\n" +
                $"\nSet the Weight of '{WalkName}' to 0.4.",
                () => IsCurrentState(IdleName)
                && TryGetState(IdleName, out AnimancerState idle)
                && TryGetState(WalkName, out AnimancerState walk)
                && idle.Weight == 0.6f
                && walk.Weight == 0.4f));

            // Pause graph.
            Steps.Add(new(
                $"Now we have a blend of both animations playing at the same time." +
                $" This sort of thing is normally achieved using Mixers." +
                $"\n" +
                $"\nNote how setting the Weight of '{WalkName}' to 0.4" +
                $" automatically changed the Weight of '{IdleName}' to 0.6 so that they add up to a total of 1." +
                $" If you had set its Weight in code it would only affect that state" +
                $" so everything can have whatever values you want," +
                $" but that's usually inconvenient when fiddling with them in the Inspector." +
                $" This feature can be disabled via '{AutoNormalizeWeightsFunction}' if you don't want it." +
                $"\n" +
                $"\nClick the Pause button at the top of the preview area to pause the character.",
                () => !_Animancer.Graph.IsGraphPlaying));

            // Unpause graph.
            Steps.Add(new(
                $"The character is now paused, but nothing else is." +
                $" You can still Right Click in the Game window and drag to move the camera around." +
                $"\n" +
                $"\nClick the Play button at the top of the preview area to resume playing.",
                () => _Animancer.Graph.IsGraphPlaying));

            // Set speed.
            Steps.Add(new(
                $"Click the '1.0x' button at the top of the preview area next to the Play/Pause button" +
                $" to show the 'Speed' slider." +
                $"\n" +
                $"\nUse that slider to set the speed to 0.5.",
                () => _Animancer.Graph.Speed == 0.5f));

            ApplyCurrentStep();
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            if (_CurrentStep >= Steps.Count)
                return;

            if (!Steps[_CurrentStep].IsComplete())
                return;

            _CurrentStep++;
            ApplyCurrentStep();
        }

        /************************************************************************************************************************/

        protected virtual void OnDestroy()
        {
            if (!_WasShowingAddAnimationField &&
                UnityEditor.EditorPrefs.GetBool(ShowAddAnimationFieldPref))
            {
                UnityEditor.EditorPrefs.SetBool(ShowAddAnimationFieldPref, false);
                Debug.Log(
                    ShowAddAnimationFieldFunction +
                    " has been disabled as it was before starting this tutorial." +
                    " Normally it would remember the value you set.");
            }
        }

        /************************************************************************************************************************/

        private void ApplyCurrentStep()
        {
            if (_CurrentStep < Steps.Count)
            {
                _Text.text = $"{_CurrentStep}/{Steps.Count} {Steps[_CurrentStep].Instructions}";
            }
            else
            {
                _Text.text = $"{Steps.Count}/{Steps.Count} Congratulations for completing this tutorial.";
                enabled = false;
            }
        }

        /************************************************************************************************************************/

        private bool IsCurrentState(string clipName)
        {
            AnimancerState state = _Animancer.States.Current;
            if (state == null)
                return false;

            AnimationClip clip = state.Clip;
            if (clip == null)
                return false;

            return clip.name == clipName;
        }

        /************************************************************************************************************************/

        private bool TryGetState(string clipName, out AnimancerState state)
        {
            foreach (AnimancerState otherState in _Animancer.States)
            {
                if (otherState.Clip.name == clipName)
                {
                    state = otherState;
                    return true;
                }
            }

            state = null;
            return false;
        }

        /************************************************************************************************************************/
#endif
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
