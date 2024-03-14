using UnityEngine;

namespace Assets.Scripts.ConstructorBonusElement
{
    [CreateAssetMenu(fileName = "Data", menuName = "Level", order = 1)]
    public class LevelPlayer : ScriptableObject
    {
        public int[] ExpLevelPlayer;
    }

}