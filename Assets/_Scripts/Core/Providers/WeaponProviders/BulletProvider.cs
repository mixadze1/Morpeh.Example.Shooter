using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Core.Providers.WeaponProviders
{
    public class BulletProvider : MonoProvider<BulletComponent>
    {
        
    }

    public struct BulletComponent : IComponent
    {
        [ReadOnly] public Vector3 Direction;
    }
}