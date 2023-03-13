using Constant;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;
using UI;

namespace Entity
{
    /// <summary>
    /// 描述：卡牌基类，用于实现拖拽卡牌的功能
    /// </summary>
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
    {
        [Header("卡牌的状态")]
        protected bool EnablePreview = true;
        private bool dragging = false;
        private bool selectMode = true;

        [Header("卡牌的初始位置和层级")]
        [SerializeField] private Vector3 savePos;
        [SerializeField] private int saveLayer;

        [Header("卡牌预览")]
        [SerializeField] private float upMove = 5f;

        //检测卡牌下的对象
        protected GameObject targetObj;

        private Canvas cv;

        private void Start()
        {
            if (GetComponent<Canvas>() != null)
                cv = GetComponent<Canvas>();
            else
                Debug.Log("未找到Canvas");

            SaveTheCardStatus();
        }

        private void Update()
        {
            if (dragging)
            {
                Vector3 mousePos = Input.mousePosition;
                transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
                DetectionObject();
            }
        }

        #region 定义方法
        //开始预览
        private void StartPreview()
        {
            transform.DOLocalMoveY(upMove, 0.05f);
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            cv.sortingOrder += 100;
        }

        //结束预览
        private void EndPreview()
        {
            transform.DOLocalMoveY(0.0f, 0.05f);
            transform.localScale = Vector3.one;
            cv.sortingOrder = saveLayer;
        }

        //保存卡牌的初始状态
        public void SaveTheCardStatus()
        {
            if (!dragging)
            {
                savePos = transform.position;
                saveLayer = cv.sortingOrder;
            }
        } 
        //拖拽预览
        private void DragPreview()
        {
            EnablePreview = false;
            StartDragPreview();
        }
        //开始拖拽预览
        private void StartDragPreview()
        {
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            cv.sortingOrder += 100;
        }
        //结束拖拽
        private void EndDrag()
        {
            //需要判断结束拖拽的位置，如果没有在可使用的范围内，则回到原来的地方
            transform.position = savePos;
            transform.localScale = Vector3.one;
            cv.sortingOrder = saveLayer;
            CheckHoverInThisCrd();
            
            EnablePreview = true;
            
        }
        #endregion


        #region 射线检测方面的方法
        protected RaycastHit2D hitInfo;
        //检测鼠标下的对象
        protected virtual void DetectionObject()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)Camera.main.transform.position;
            Ray2D ray2D = new Ray2D(mousePosition, direction.normalized);

            hitInfo = Physics2D.Raycast(ray2D.origin, ray2D.direction, 100f, LayerMask.GetMask("Entity"));

            //剩下判断当前卡牌下是否为卡牌所使用对象，留给子类重写

        }

        //清空检测记录，留给子类重写
        protected virtual void ClearTheRecord() { }

        //启用高亮
        protected virtual void EnableHighlighting() { }

        //关闭高亮
        protected virtual void DisableHighlighting() { }


        //检测结束拖拽的位置是否还有其他UI对象
        private void CheckHoverInThisCrd()
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            foreach (RaycastResult go in raycastResults)
            {
                if (go.gameObject == gameObject)
                {
                    StartPreview();
                }
            }
        } 
        #endregion


        #region 实现接口
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (EnablePreview)
                StartPreview();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (EnablePreview)
                EndPreview();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            //点击鼠标左键，开启拖拽
            if(selectMode && eventData.button == PointerEventData.InputButton.Left)
            {
                if (!dragging)
                {
                    dragging = true;
                    DragPreview();
                }
            }
            //点击鼠标右键,交给子类重写
            
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                dragging = true;
                selectMode = false;
                DragPreview();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            selectMode = true;
            dragging = false;
            ClearTheRecord();
            EndDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            DetectionObject();
        }
        #endregion

    }

}