using Assets.Scripts.ConstructorBonusElement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class UpdateBonusPlayer : MonoBehaviour
    {
        //TODO Add branch
        public static Action<Bonus> UpdateStatePlayer { get; set; }
        [SerializeField] private List<Bonus> _CurrentBonusList; //TEST Visibility
        private ControllerBonus _controllerBonus;
        private const int NUMBER_CARD_PER_LEVEL_UPDATE = 3;
        private int[] CurrentRandomValue = new int[3];

        List<Bonus> CurrentBonusToPanel = new List<Bonus>();
        private void Awake()
        {
            _controllerBonus = GetComponent<ControllerBonus>();
        }

        public void  UpdateBonus()
        {
            var UiData = UIData.instanse;
            _CurrentBonusList = _controllerBonus.CurrentBonus;
            CurrentBonusToPanel.Clear();

            ClearCurrentRandomValue();
            RandomValueIndexForBonus();

            for (int i = 0; i < NUMBER_CARD_PER_LEVEL_UPDATE; i++)
            {
                int IndexRandomCard = CurrentRandomValue[i];

                UiData.AllLevelUiData[i].NameBonus.text = _CurrentBonusList[IndexRandomCard].NameBonus;
                UiData.AllLevelUiData[i].Description.text = _CurrentBonusList[IndexRandomCard].Description;
                UiData.AllLevelUiData[i].BonusImage.sprite = _CurrentBonusList[IndexRandomCard].BonusImage;
                UiData.AllLevelUiData[i].Frame.sprite = _CurrentBonusList[IndexRandomCard].Frame;

                UiData.AllLevelUiData[i].ElementBonus = _CurrentBonusList[IndexRandomCard];

                CurrentBonusToPanel.Add(_CurrentBonusList[IndexRandomCard]);
            }

        }

        private void RandomValueIndexForBonus()
        { 
            bool isRepeatingNumber = false;
            int CurrentCountnegativeValue = 0;

            while (CurrentCountnegativeValue != 3)
            {
                isRepeatingNumber = false;
                CurrentCountnegativeValue = 0;
                var IndexRandomCard = UnityEngine.Random.Range(0, _CurrentBonusList.Count - 1);
                for (int b = 0; b < CurrentRandomValue.Length; b++)
                {
                    if (CurrentRandomValue[b] == IndexRandomCard)
                    {
                        isRepeatingNumber = true;
                        break;
                    }
                   
                }

                if (!isRepeatingNumber)
                {
                    for (int t = 0; t < CurrentRandomValue.Length; t++)
                    {
                        if (CurrentRandomValue[t] == -1)
                        {
                            CurrentRandomValue[t] = IndexRandomCard;
                            break;
                        }
                    }
                }
                for (int y = 0; y < CurrentRandomValue.Length; y++)
                {
                    if (CurrentRandomValue[y] != -1)
                    {
                        CurrentCountnegativeValue++;
                    }
                }
            }
        }

        private void ClearCurrentRandomValue()
        {
            for (int i = 0; i < CurrentRandomValue.Length; i++)
            {
                CurrentRandomValue[i] = -1;
            }
        }

        public void ClickToCurrentBonus(int BonusID)
        {
            UpdateStatePlayer?.Invoke(CurrentBonusToPanel[BonusID]);
            
        }
    }
}