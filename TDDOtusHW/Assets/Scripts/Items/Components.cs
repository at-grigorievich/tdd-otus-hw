using System;
using ATG.OtusHW.Inventory;

namespace ATG.Items
{
    public interface IItemComponent
    {
        IItemComponent Clone();
    }

    [Serializable]
    public class StackableItemComponent: IItemComponent
    {
        public int Count;
        public int MaxCount;
        
        public IItemComponent Clone()
        {
            return new StackableItemComponent() { Count = 1, MaxCount = MaxCount };
        }
    }
    
    public abstract class HeroEffectComponent : IItemComponent
    {
        public abstract IItemComponent Clone();
        public abstract void AddEffect(Hero hero);
        public abstract void RemoveEffect(Hero hero);
    }
    
    [Serializable]
    public class HeroDamageEffectComponent : HeroEffectComponent
    {
        public int DamageEffect = 2;
        
        public override IItemComponent Clone()
        {
            return new HeroDamageEffectComponent()
            {
                DamageEffect = this.DamageEffect
            };
        }

        public override void AddEffect(Hero hero)
        {
            hero.damage += DamageEffect;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.damage -= DamageEffect;
        }
    }
    
    [Serializable]
    public class HeroHitPointsEffectComponent : HeroEffectComponent
    {
        public int HitPointsEffect = 2;
        
        public override IItemComponent Clone()
        {
            return new HeroHitPointsEffectComponent()
            {
                HitPointsEffect = this.HitPointsEffect
            };
        }

        public override void AddEffect(Hero hero)
        {
            hero.hitPoints += HitPointsEffect;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.hitPoints -= HitPointsEffect;
        }
    }
    
    [Serializable]
    public class HeroSpeedEffectComponent : HeroEffectComponent
    {
        public int SpeedEffect = 2;
        
        public override IItemComponent Clone()
        {
            return new HeroSpeedEffectComponent()
            {
                SpeedEffect = this.SpeedEffect
            };
        }

        public override void AddEffect(Hero hero)
        {
            hero.speed += SpeedEffect;
        }

        public override void RemoveEffect(Hero hero)
        {
            hero.speed -= SpeedEffect;
        }
    }

    [Serializable]
    public class HeroEquipmentComponent : IItemComponent
    {
        public EquipType Tag;
        
        public IItemComponent Clone()
        {
            return new HeroEquipmentComponent()
            {
                Tag = this.Tag
            };
        }
    }
}