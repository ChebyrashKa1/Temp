using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class CardVisualItem : MonoBehaviour
    {
        [SerializeField] private RectTransform          thisTransform;
        [Space]
        [SerializeField] private TMPro.TMP_Text         txtTitle;
        [SerializeField] private TMPro.TMP_Text         txtDesc;
        [SerializeField] private TMPro.TMP_Text         txtDamageValue;
        [SerializeField] private Button                 button;
        [SerializeField] private Image                  shirt;
        [SerializeField] private Image                  cardIcon;
        [Space]
        [SerializeField] private TweenPositionUI        tweenPosition;
        [SerializeField] private TweenRotation          tweenRotation;

        public CardData CardDatas { get; set; }
        public Vector2 CardPosition
        {
            get
            {
                return thisTransform.anchoredPosition;
            }
            set
            {
                thisTransform.anchoredPosition = value;
            }
        }

        public bool isPlayer = false;

        public void InitCard(CardData data, Vector2 targetPos, bool player)
        {
            UnlockCard(false);
            InitData(data, player);



            StartCoroutine(MoveCard(targetPos, () =>
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => { StartCoroutine(SelectCard()); });
            }, 0.25f));
        }

        #region Start Init (Hand out)
        private void InitData(CardData data, bool player)
        {
            isPlayer = player;
            gameObject.SetActive(true);
            CardDatas = data;

            txtTitle.SetText(data.TitleCard);
            UpdateValue(-1);
            //txtDesc.SetText(data.DescCard);
            cardIcon.sprite = data.Icon;
        }
        #endregion

        public void UpdateValue(float valueDamage)
        {
            if (valueDamage < 0)
            {
                txtDamageValue.SetText("0");
                return;
            }
            txtDamageValue.SetText(valueDamage.ToString("0"));
        }

        public IEnumerator SelectCard()//RectTransform holder)
        {
            var battleTurn = GameCore.battle;

            #region rotate
            battleTurn.Window.RotateAfterSelectAll(isPlayer);
            //StartCoroutine(RotateCard(270, ActionRotateBack));
            #endregion

            Vector2 targetPos = battleTurn.Window.PositionPlayersCardMiddle;
            if (!isPlayer)
            {
                targetPos = battleTurn.Window.PositionEnemiesCardMiddle;
            }
            else
            {
                battleTurn.Window.LockAllCard();
                if (battleTurn.UnitAttack as PlayerUnit)
                    battleTurn.Window.CardAttack = this;
                else if (battleTurn.UnitDefense as PlayerUnit)
                    battleTurn.Window.CardDefense = this;
            }

            yield return StartCoroutine(MoveCard(targetPos));

           
            if (isPlayer)
            {
                battleTurn.EndPlayerSelectCard = true;
                //Типса что делать
               // battleTurn.Window.TurnStepsTips(TurnSteps.TapEndTurn);
            }
           
        }

        public IEnumerator MoveCard(Vector2 targetPos, System.Action action = null, float duration = 0.5f, System.Action actionStart = null)
        {
            tweenPosition.duration = duration;

            #region Start Action
            EventDelegate eventDelegate = new EventDelegate(
                ()=>{
                    actionStart?.Invoke();
            });
            //tweenPosition.RemoveOnFinished(eventDelegate);
            tweenPosition.AddOnStart(eventDelegate);
            #endregion

            tweenPosition.SetStartToCurrentValue();
            tweenPosition.ResetToBeginning();
            tweenPosition.to.Set(targetPos.x, targetPos.y);
            tweenPosition.PlayForward();
            tweenPosition.SetOnFinished(()=> { action?.Invoke(); });

            tweenPosition.ClearEventsOnFinish = true;
            tweenPosition.ClearEventsOnStart = true;

            yield return GameCore.yields.WaitFor(tweenPosition.duration);
        }

        #region rotate
        public void RotateAfterSelectSingle()
        {
            StartCoroutine(RotateCard(270,ActionRotateBack));
        }

        private IEnumerator RotateCard(int toY, System.Action action = null, float duration = 0.25f, int toZ = 0)
        {
            tweenRotation.duration = duration;

            EventDelegate eventDelegate = new EventDelegate(
           () => {
               action?.Invoke();
           });

            tweenRotation.RemoveOnFinished(eventDelegate);

            tweenRotation.SetStartToCurrentValue();
            tweenRotation.ResetToBeginning();
            tweenRotation.to.Set(0, toY, toZ);
            tweenRotation.PlayForward();
            tweenRotation.SetOnFinished(eventDelegate);

            yield return GameCore.yields.WaitFor(tweenRotation.duration);
        }

        private void ActionRotateForward()
        {
            shirt.gameObject.SetActive(true);   //flip shirt
            tweenRotation.ClearEventsOnFinish = true;
            StartCoroutine(RotateCard(180)); // 180
        }
        private void ActionRotateBack()
        {
            shirt.gameObject.SetActive(false);   //flip shirt
            tweenRotation.ClearEventsOnFinish = true;
            StartCoroutine(RotateCard(360));
        }

        public void RotateWhenCardOnCard(int toZ)
        {
            thisTransform.rotation = Quaternion.Euler(Vector3.zero);
            StartCoroutine(RotateCard(360, null, 0.5f, toZ));
        }
        #endregion


        public void DestroyCard(Vector2 newPosition)
        {
            gameObject.SetActive(false);
            CardTeleportToRestart(newPosition);
        }

        public void UnlockCard(bool unlock)
        {
            button.interactable = unlock;
        }

        public void CardTeleportToRestart(Vector2 newPosition)
        {
            thisTransform.anchoredPosition = newPosition;
            thisTransform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            shirt.gameObject.SetActive(false);   //flip shirt
        }

        private void ShowInfo()
        {
            Dbg.Log("ShowInfo title: " + txtTitle, Color.green);
        }

        public IEnumerator ShuffleCard(float newX)
        {
            yield return StartCoroutine(MoveCard(new Vector2(0, thisTransform.anchoredPosition.y), null, 0.5f, ()=> {
                StartCoroutine(RotateCard(90, ActionRotateForward)); //90
            }));
            yield return StartCoroutine(MoveCard(new Vector2(newX, thisTransform.anchoredPosition.y), null, 0.25f)); 
        }

    }
}