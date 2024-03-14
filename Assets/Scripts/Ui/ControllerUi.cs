using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Ui
{
    public class ControllerUi : MonoBehaviour
    {
        public static ControllerUi instanse;
        public PlayerStatesHolder playerStatesHolder;
        private UpdateBonusPlayer _updateBonusPlayer;

        private UIData _uIData;

        private bool _isAcriveWriteImage;
        private bool _isAcriveRedImage;

        public static int CountPlayerExp; //Max = 10;
        private int LevelExp = 1;

        private const int CountAddLevel = 1;
        private int CountDeadEnemy;

        [SerializeField] private int AllTime = 10;
        private int CurrentTimeSesion;

        public int CurrentLevelPlayer;
        private bool isDeadPlayer = false;

        private void Awake()
        {
            if (instanse != null) Destroy(instanse);
            instanse = this;
            _updateBonusPlayer = GetComponent<UpdateBonusPlayer>();
        }

        private void Start()
        {
            _uIData = UIData.instanse;

            PlayerStatesHolder.EventMinusPlayerHp += MinusHpPlayer;
            PlayerStatesHolder.EventToDeadPlayer += DeadOrWinPlayer;


            CurrentTimeSesion = AllTime * 60;
            StartCoroutine(Timer());
        }

        private void OnDestroy()
        {
            PlayerStatesHolder.EventMinusPlayerHp -= MinusHpPlayer;
            PlayerStatesHolder.EventToDeadPlayer -= DeadOrWinPlayer;

        }

        private IEnumerator Timer()
        {
            while (CurrentTimeSesion > 0)
            {
                CurrentTimeSesion -= 1;
                (int, int) TimeSpan = ConvertingValuesToTime(CurrentTimeSesion);
                UIData.instanse.Timer.text = $"{TimeSpan.Item1}:{TimeSpan.Item2}";
                yield return new WaitForSeconds(1f);
            }
            //EndTime
            StopCoroutine(Timer());
            DeadOrWinPlayer(false);

        }

        private (int,int) ConvertingValuesToTime(int CurrentTimeSesions)
        {
            int minutes = CurrentTimeSesions / 60;
            int seconds = CurrentTimeSesions % 60;
            return (minutes, seconds);
        }

        public void SetRedAim()
        {
            if (_isAcriveRedImage)
            {
                StopCoroutine(SetRedImage(_uIData.RedAim.gameObject, _isAcriveRedImage));
                _uIData.WriteAim.gameObject.SetActive(true);
            }

            StartCoroutine(SetRedImage(_uIData.RedAim.gameObject, _isAcriveRedImage));

            CountDeadEnemy++;
        }
        public void SetWriteAim()
        {
            if (_isAcriveRedImage)
            {
                StopCoroutine(SetRedImage(_uIData.WriteAim.gameObject, _isAcriveWriteImage));
                _uIData.WriteAim.gameObject.SetActive(false);
            }
            StartCoroutine(SetRedImage(_uIData.WriteAim.gameObject, _isAcriveWriteImage));
        }
        
        private IEnumerator SetRedImage(GameObject AimImage,bool isActive)
        {
            isActive = true;
            AimImage.SetActive(true);
            AimImage.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(0.25f);
            AimImage.SetActive(false);
            isActive = false;
        }

        private void MinusHpPlayer()
        {


            ///ЧЕЕЕЕЕЕЕЕЕЛЛЛЛЛЛЛЛ ?????
            /*            var DataUI = UIData.instanse;
                        for (int i = DataUI.hpbar.Count -1; i < DataUI.hpbar.Count; i--)
                        {
                            if (0 > i)
                            {
                                return;
                            }
                            if (DataUI.hpbar[i].enabled ==true)
                            {
                                DataUI.hpbar[i].enabled = false;
                                return;
                            }
                        }*/


            var DataUI = UIData.instanse;
            if(DataUI.hpbar.Count>0)
            {
                Destroy(DataUI.hpbar[DataUI.hpbar.Count-1]);
                DataUI.hpbar.RemoveAt(DataUI.hpbar.Count-1);
            }
        }

        public bool PlusHpPlayer()
        {
            var DataUI = UIData.instanse;
            if (playerStatesHolder.currentHealth != playerStatesHolder.maxHealthPoints)
            {
                GameObject GObject = Instantiate(DataUI.hpBarPrefab);
                GObject.transform.SetParent(DataUI.maxHpBar[0].transform.parent);
                DataUI.hpbar.Add(GObject);
                return true;
            }
            return false;
            

        }
        public void PlusMaxHpPlayer()
        {
            var DataUI = UIData.instanse;
            GameObject GObject = Instantiate(DataUI.maxHpBarPrefab);
            GObject.transform.SetParent(DataUI.maxHpBarContainer.transform);
            DataUI.maxHpBar.Add(GObject);
            GameObject gObject = Instantiate(DataUI.hpBarPrefab);
            gObject.transform.SetParent(DataUI.hpBarContainer.transform);
            DataUI.hpbar.Add(gObject);
            
        }
        private void DeadOrWinPlayer(bool isDead)
        {
            UIData.instanse.EndPanel.SetActive(true);
            UIData.instanse.DeadImage.SetActive(isDead);
            UIData.instanse.DeadImage.SetActive(!isDead);

            if (isDead && !isDeadPlayer)
            {
                FindObjectOfType<PMoveController>().GetComponent<PlayerInput>().enabled = false;

                isDeadPlayer = true;
                EndGame();
                CurrentTimeSesion = 0;
                StopCoroutine(Timer());
            }


           
        }

        //TODO Changes альфа канал - изменение прозрачности.
        //private IEnumerator blackoutPanel()
        //{
        //    int Currentblackout;
        //    while (true)
        //    {
        //        WriteSprite.color = new Color(WriteSprite.color.r, WriteSprite.color.g, WriteSprite.color.b, WriteSprite.color.a - (SpeedColoring * Time.deltaTime));

        //        OriginalSprite.color = new Color(OriginalSprite.color.r, OriginalSprite.color.g, OriginalSprite.color.b, OriginalSprite.color.a + (SpeedColoring * Time.deltaTime));
        //        Currentblackout = 
        //        yield return new WaitForSeconds(0.1f);

        //    }
        //}

        private void EndGame()
        {
            int CurrentGamePlayTime = AllTime * 60 - CurrentTimeSesion;
            (int,int) TimeSpan = ConvertingValuesToTime(CurrentGamePlayTime);
            int ValueReward =  RewardPlayer(CurrentGamePlayTime, CountDeadEnemy);

            UIData.instanse.CountTimeSesion.text = $"Final Time: {TimeSpan.Item1}:{TimeSpan.Item2}";
            UIData.instanse.CountDeadEnemy.text = $"Dead Enemy: {CountDeadEnemy}";
            UIData.instanse.CountRewardPlayer.text = ValueReward.ToString();
            OnEnableCursor();
          
        }
        
        private int RewardPlayer(int TimeGamePlay, int DeadEnemy)
        {
            int Reward = TimeGamePlay + DeadEnemy;
            return Reward;
        }

        public void ChangesLevelBar()
        {
            int ExpToNewLevel = UIData.instanse.levelPlayer.ExpLevelPlayer[CurrentLevelPlayer];
            int AddPlayerExp = CountAddLevel;
            CountPlayerExp += AddPlayerExp;

            if (CountPlayerExp >= ExpToNewLevel)
            {

                CountPlayerExp -= ExpToNewLevel;
                LevelExp++;
                _uIData.Level.text = LevelExp.ToString();
                CurrentLevelPlayer++;
                UpdateUpLevel();
            }

           
            float CountAddExp = CountPlayerExp;
            CountAddExp = CountAddExp / ExpToNewLevel;
            _uIData.LevelBar.fillAmount = CountAddExp;
        }

        public void ResetScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }

        public void UpdateUpLevel()
        {
            UIData.instanse.BakeLevel.SetActive(true);

            _updateBonusPlayer.UpdateBonus();
            _uIData.PanelUpgradeLevel.SetActive(true);

            this.Invoke("FalsePanel", 0.1f);
            Time.timeScale = 0.1f;
        }

        public void ClickToLevelUpBonus(int CurrentBonus)
        {
            _updateBonusPlayer.ClickToCurrentBonus(CurrentBonus);
            _uIData.PanelUpgradeLevel.SetActive(false);
            OnDisableCursor();
            
            Time.timeScale = 1;

        }

        public void FalsePanel()
        {
            UIData.instanse.BakeLevel.SetActive(false);

            OnEnableCursor();
        }

        private void OnDisableCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnableCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    
}