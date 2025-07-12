using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace _Scripts.Core.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthProvider : MonoProvider<HealthComponent> 
    {
    
    }
}