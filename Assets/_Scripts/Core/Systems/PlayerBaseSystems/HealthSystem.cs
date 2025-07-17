using _Scripts.Core.Configs;
using _Scripts.Core.Providers;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthSystem : ISystem
    {
        private HealthConfig _healthConfig;
        private Filter _filter;
        private Stash<HealthComponent> _healthStash;
        public World World { get; set;}

        public HealthSystem(HealthConfig healthConfig)
        {
            _healthConfig = healthConfig;
        }

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
            }
        }

        public void Dispose()
        {

        }
    }
}