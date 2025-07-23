
using System;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Core.Providers.WeaponProviders
{
    [RequireComponent(typeof(TransformProvider))]
    public class DecalProvider : MonoProvider<DecalComponent>
    {
        
    }

    [Serializable]
    public struct DecalComponent : IComponent
    {
       [ReadOnly] public float Lifetime;
       [ReadOnly] public float TimeAlive;
    }
}