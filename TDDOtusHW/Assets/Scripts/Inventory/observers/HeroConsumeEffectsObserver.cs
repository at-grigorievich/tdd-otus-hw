using System;
using System.Collections.Generic;

namespace ATG.Items.Inventory
{
    public class HeroConsumeEffectsObserver: IDisposable
    {
        private readonly Inventory _inventory;
        private readonly Hero _hero;
        
        public HeroConsumeEffectsObserver(Inventory inventory, Hero hero)
        {
            _inventory = inventory;
            _hero = hero;
            
            _inventory.OnItemConsumed += OnItemConsumed;
        }
        
        private void OnItemConsumed(Item item)
        {
            if(item.TryGetComponents(out IEnumerable<HeroEffectComponent> effects) == false) return;
            
            foreach (var heroEffectComponent in effects)
            {
                heroEffectComponent.AddEffect(_hero);
            }
        }
        
        public void Dispose()
        {
            _inventory.OnItemConsumed -= OnItemConsumed;
        }
    }
}