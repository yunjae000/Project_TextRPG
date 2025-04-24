namespace TextRPG
{
    interface IDamagable
    {
        public void OnDamage(AttackType type, float damage, bool isSkill);
    }

    interface IWearable
    {
        public void OnEquip(Character character);
        public void OnUnequip(Character character);
    }

    interface IUseable
    {
        public void OnUsed(Character character);
        public void OnDeBuffed(Character character);
    }

    interface IPurchasable
    {
        public void OnPurchase(Character character);
    }

    interface ISellable
    {
        public void OnSell(Character character);
    }

    interface IPickable
    {
        public void OnPicked(Character character);
        public void OnDropped(Character character);
    }

    interface IContractable
    {
        public void OnContracted();
        public void OnContracted(Character character);
        public void OnProgress();
        public void OnProgress(Character character);
        public void OnCompleted(Character character);
    }

    interface ICancelable
    {
        public void OnCanceled(Character character);
    }

    interface ISkillActive
    {
        public bool OnActive(Character character, Monster target);
        public bool OnActive(Character character, LinkedList<Monster> targets);
    }

    interface ISkillBuff
    {
        public bool OnActive(Character character);
        public void OnBuffExpired(Character character);
    }
}
