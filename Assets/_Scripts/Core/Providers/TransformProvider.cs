using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Scripts.Core
{
    public class TransformProvider : MonoProvider<TransformComponent>
    {
        
    }
    
    [Serializable]
    public struct TransformComponent : IComponent
    {
        public Transform Transform;    
        public Vector3 Direction;      
    }
}