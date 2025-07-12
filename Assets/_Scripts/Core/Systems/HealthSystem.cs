using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class HealthSystem : ISystem 
{    
    private Filter _filter;
    private Stash<HealthComponent> _healthStash;
    public World World { get; set;}

    public void OnAwake()
    {
        _filter = World.Filter.With<HealthComponent>().Build();
        _healthStash = World.GetStash<HealthComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        foreach (var entity in _filter)
        {
            ref var healthComponent = ref _healthStash.Get(entity);
            Debug.Log(healthComponent.HealthPoints);
        }
    }

    public void Dispose()
    {

    }
}