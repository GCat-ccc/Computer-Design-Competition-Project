using Constant;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;
using UI;

namespace Entity
{
    /// <summary>
    /// ���������ƻ��࣬����ʵ����ק���ƵĹ���
    /// </summary>
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
    {
        [Header("���Ƶ�״̬")]
        protected bool EnablePreview = true;
        private bool dragging = false;
        private bool selectMode = true;

        [Header("���Ƶĳ�ʼλ�úͲ㼶")]
        [SerializeField] private Vector3 savePos;
        [SerializeField] private int saveLayer;

        [Header("����Ԥ��")]
        [SerializeField] private float upMove = 5f;

        //��⿨���µĶ���
        protected GameObject targetObj;

        private Canvas cv;

        private void Start()
        {
            if (GetComponent<Canvas>() != null)
                cv = GetComponent<Canvas>();
            else
                Debug.Log("δ�ҵ�Canvas");

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

        #region ���巽��
        //��ʼԤ��
        private void StartPreview()
        {
            transform.DOLocalMoveY(upMove, 0.05f);
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            cv.sortingOrder += 100;
        }

        //����Ԥ��
        private void EndPreview()
        {
            transform.DOLocalMoveY(0.0f, 0.05f);
            transform.localScale = Vector3.one;
            cv.sortingOrder = saveLayer;
        }

        //���濨�Ƶĳ�ʼ״̬
        public void SaveTheCardStatus()
        {
            if (!dragging)
            {
                savePos = transform.position;
                saveLayer = cv.sortingOrder;
            }
        } 
        //��קԤ��
        private void DragPreview()
        {
            EnablePreview = false;
            StartDragPreview();
        }
        //��ʼ��קԤ��
        private void StartDragPreview()
        {
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            cv.sortingOrder += 100;
        }
        //������ק
        private void EndDrag()
        {
            //��Ҫ�жϽ�����ק��λ�ã����û���ڿ�ʹ�õķ�Χ�ڣ���ص�ԭ���ĵط�
            transform.position = savePos;
            transform.localScale = Vector3.one;
            cv.sortingOrder = saveLayer;
            CheckHoverInThisCrd();
            
            EnablePreview = true;
            
        }
        #endregion


        #region ���߼�ⷽ��ķ���
        protected RaycastHit2D hitInfo;
        //�������µĶ���
        protected virtual void DetectionObject()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)Camera.main.transform.position;
            Ray2D ray2D = new Ray2D(mousePosition, direction.normalized);

            hitInfo = Physics2D.Raycast(ray2D.origin, ray2D.direction, 100f, LayerMask.GetMask("Entity"));

            //ʣ���жϵ�ǰ�������Ƿ�Ϊ������ʹ�ö�������������д

        }

        //��ռ���¼������������д
        protected virtual void ClearTheRecord() { }

        //���ø���
        protected virtual void EnableHighlighting() { }

        //�رո���
        protected virtual void DisableHighlighting() { }


        //��������ק��λ���Ƿ�������UI����
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


        #region ʵ�ֽӿ�
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
            //�����������������ק
            if(selectMode && eventData.button == PointerEventData.InputButton.Left)
            {
                if (!dragging)
                {
                    dragging = true;
                    DragPreview();
                }
            }
            //�������Ҽ�,����������д
            
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