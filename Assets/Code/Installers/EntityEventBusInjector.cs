using System;
using Code.EntityModule;
using Code.Services.EventService;

namespace Code.Installers
{
    public class EntityEventBusInjector
    {
        private readonly IEventBus _eventBus;

        public EntityEventBusInjector(IEventBus eventBus)
        {
            _eventBus = eventBus;
            Entity.OnNewEntityInject += Inject;
        }

        private void Inject(Action<IEventBus> injection)
        {
            injection?.Invoke(_eventBus);
        }
    }
}