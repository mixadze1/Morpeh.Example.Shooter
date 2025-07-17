using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.Collections;

namespace _Scripts.Core.Providers.PlayerProviders
{
    public class SpeedProvider : MonoProvider<SpeedComponent>
    {
        
    }

    public struct SpeedComponent : IComponent
    {
       [ReadOnly] public bool IsMoving;
        public event Action OnStartMove;
        public event Action OnEndMove;
    }
}