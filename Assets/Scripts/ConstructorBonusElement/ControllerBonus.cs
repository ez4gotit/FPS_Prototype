using Assets.Scripts.Ui;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ConstructorBonusElement
{
    public class ControllerBonus : MonoBehaviour
    {
        [SerializeField] private List<Bonus> AllBonus;
        public List<Bonus> CurrentBonus { get; private set; } = new List<Bonus>();
        private List<Bonus> RemoveList { get; set; } = new List<Bonus>();
        [SerializeField] private int[] CurrentLevelBonus = new int[8];

        const int MAX_BONUS_LEVEL = 4;
        const int CURRENT_INDEX_SPAWN_NEW_CARD = 3;
        public int LengthBranchBonus;

        private void Start()
        {
            UpdateBonusPlayer.UpdateStatePlayer += BonusSelection;
            InitBonus();
        }

        private void OnDestroy()
        {
            UpdateBonusPlayer.UpdateStatePlayer -= BonusSelection;
        }

        private void InitBonus()
        {
            AllBonus = UIData.instanse.AllBonus;
            for (int i = 0; i < AllBonus.Count; i++)
            {
                if (AllBonus[i].SaveCurrentBonus.RangeToSelectionBonus == 0)
                {
                    CurrentBonus.Add(AllBonus[i]);
                    RemoveList.Add(AllBonus[i]);
                }
            }
            RemoveBonus();
        }


        /// <summary>
        /// После выбора текущего навыка идёт обновления текущих рангов.
        /// </summary>
        /// <param name="improvements"> текущий выбранный бонус</param>
        public void BonusSelection(Bonus bonus) //TODO Передать бонус и изменить статы.
        {
            Bonus _bonus = new Bonus();
            _bonus.SaveCurrentBonus.improvements = bonus.SaveCurrentBonus.improvements;
            _bonus.SaveCurrentBonus.RangeToSelectionBonus = bonus.SaveCurrentBonus.RangeToSelectionBonus;
            PlayerStatesHolder.currentBonus.Add(_bonus);
            Debug.Log(PlayerStatesHolder.currentBonus[PlayerStatesHolder.currentBonus.Count-1].SaveCurrentBonus);
            Improvements improvements = bonus.SaveCurrentBonus.improvements;
            int Index = (int)improvements;
            var LevelBonus = ++CurrentLevelBonus[Index]; // повышаем текущий левел выпадения бонуса.
            Debug.Log("Level Bonus " + LevelBonus);//CurrentLevelBonus[Index]


            for (int i = 0; i < AllBonus.Count; i++)
            {
                if (AllBonus[i].SaveCurrentBonus.improvements == improvements && AllBonus[i].SaveCurrentBonus.RangeToSelectionBonus == LevelBonus)
                {
                    CurrentBonus.Add(AllBonus[i]);
                    RemoveList.Add(AllBonus[i]);
                }
                if (AllBonus[i].SaveCurrentBonus.improvements == improvements &&
                    AllBonus[i].SaveCurrentBonus.RangeToSelectionBonus > LevelBonus && 
                    AllBonus[i].SaveCurrentBonus.RangeToSelectionBonus != CURRENT_INDEX_SPAWN_NEW_CARD)
                {
                    CurrentBonus.Add(AllBonus[i]);
                    RemoveList.Add(AllBonus[i]);
                }
            }

            for (int i = 0; i < CurrentBonus.Count; i++)
            {
                if (CurrentBonus[i] == bonus)
                {
                    CurrentBonus.Remove(CurrentBonus[i]);
                    break;
                }
            }

            RemoveBonus();
        }
        private void RemoveBonus()
        {
            for (int i = 0; i < RemoveList.Count; i++)
            {
                if (RemoveList[i].SaveCurrentBonus.RangeToSelectionBonus != MAX_BONUS_LEVEL)
                {
                    AllBonus.Remove(RemoveList[i]);
                }
            }
            RemoveList.Clear();
        }
    }
}
