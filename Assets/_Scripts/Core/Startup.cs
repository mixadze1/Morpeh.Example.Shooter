using Scellecs.Morpeh;
using UnityEngine;

namespace _Scripts.Core
{
    public class Startup : MonoBehaviour
    {
        private World world;
    
        private void Start() {
            this.world = World.Default;
        
            var systemsGroup = this.world.CreateSystemsGroup();
            systemsGroup.AddSystem(new HealthSystem());
        
            this.world.AddSystemsGroup(order: 0, systemsGroup);
        }
    }
}
