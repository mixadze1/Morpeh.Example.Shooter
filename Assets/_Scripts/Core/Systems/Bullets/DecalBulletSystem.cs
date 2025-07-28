using _Scripts.Core.Providers.PlayerProviders;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace _Scripts.Core.Systems.Bullets
{
    public class DecalBulletSystem : ISystem
    {
        private Filter _filter;
        private Stash<DecalComponent> _decalStash;
        private Stash<TransformComponent> _transformStash;

        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter.With<DecalComponent>().Build();
            _decalStash = World.GetStash<DecalComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var decal = ref _decalStash.Get(entity);
                decal.AddTimeAlive(deltaTime);

                ref var transformComponent = ref _transformStash.Get(entity);

                if (decal.IsEndLife())
                {
                    Object.Destroy(transformComponent.Transform.gameObject);
                }
            }
        }

        public void Dispose() { }
    }
}