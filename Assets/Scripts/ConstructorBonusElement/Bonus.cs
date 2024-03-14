using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ConstructorBonusElement
{
    public enum Improvements { BulletHell, Runner, ImpulseShmimpulse, ExpMaster, BigBoy, DeathLover, WalkTalk, AccurateKiller }
    [System.Serializable]
    public struct SaveCurrentBonus
    {
        public Improvements improvements;
        public int RangeToSelectionBonus;
    }

    [CreateAssetMenu(fileName = "Data", menuName = "Bonus", order = 1)]
    public class Bonus : ScriptableObject
    {
        public string NameBonus;
        public string Description;
        public Sprite BonusImage;
        public Sprite Frame;
        public SaveCurrentBonus SaveCurrentBonus;
        private float TestBonusPercent;
        private int TestBonus;

    }

   

}