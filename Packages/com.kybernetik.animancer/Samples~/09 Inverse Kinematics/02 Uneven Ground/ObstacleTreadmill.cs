// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer.Samples.InverseKinematics
{
    /// <summary>Spawns a bunch of obstacles and randomises them each time the target moves too far away.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/ik/uneven-ground">
    /// Uneven Ground</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.InverseKinematics/ObstacleTreadmill
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Inverse Kinematics - Obstacle Treadmill")]
    [AnimancerHelpUrl(typeof(ObstacleTreadmill))]
    public class ObstacleTreadmill : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        [SerializeField] private float _SpawnCount = 20;
        [SerializeField] private Material _ObstacleMaterial;
        [SerializeField, Meters] private float _Length = 6;
        [SerializeField, Degrees] private float _RotationVariance = 45;
        [SerializeField, Multiplier] private float _BaseScale = 0.8f;
        [SerializeField, Multiplier] private float _ScaleVariance = 0.3f;
        [SerializeField] private Transform _Target;

        /************************************************************************************************************************/

        private readonly List<Transform> Obstacles = new();

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            // Spawn a bunch of obstacles and randomize their layout.
            for (int i = 0; i < _SpawnCount; i++)
            {
                Transform capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule).transform;
                capsule.GetComponent<Renderer>().sharedMaterial = _ObstacleMaterial;
                capsule.parent = transform;
                Obstacles.Add(capsule);
            }

            ScrambleObjects();
        }

        /************************************************************************************************************************/

        private void ScrambleObjects()
        {
            // Move and rotate each of the obstacles randomly.
            for (int i = 0; i < Obstacles.Count; i++)
            {
                Transform obstacle = Obstacles[i];
                obstacle.SetLocalPositionAndRotation(
                    new Vector3(
                        Random.Range(0, _Length),
                        0,
                        0),
                    Quaternion.Euler(
                        90,
                        Random.Range(-_RotationVariance, _RotationVariance),
                        0));

                float size = _BaseScale + Random.Range(-_ScaleVariance, _ScaleVariance);
                obstacle.localScale = Vector3.one * size;
            }
        }

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            // When the target moves too far, teleport them back and randomize the obstacles again.
            Vector3 position = _Target.position;
            if (position.x < transform.position.x)
            {
                ScrambleObjects();

                position.x += _Length;

                // Adjust the height to make sure it's above the ground.
                position.y += 5;
                if (Physics.Raycast(position, Vector3.down, out RaycastHit raycastHit, 10))
                    position = raycastHit.point;

                _Target.position = position;
            }
        }

        /************************************************************************************************************************/

        [SerializeField]
        private Transform _Ground;

        // Set by a UI Slider.
        public float Slope
        {
            get => _Ground.localEulerAngles.z;
            set => _Ground.localEulerAngles = new Vector3(0, 0, value);
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingPhysics3DModuleError(this);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
