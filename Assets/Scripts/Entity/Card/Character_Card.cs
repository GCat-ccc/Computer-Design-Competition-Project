using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using UnityEngine.EventSystems;

namespace Entity
{
    /// <summary>
    /// ��������ɫ���Ƽ��������Ŀ��Ƶ��ߣ���UI���ϣ��Ǳ������ק��������ʹ�õ�
    /// </summary>
    public class Character_Card :Card
    {

//��д����ķ���//
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                if(targetObj != null && targetObj.GetComponent<PlacablePoint>().CheckMouseOnTargetAndObstacle())
                {
                    targetObj.GetComponent<PlacablePoint>().PlaceCharacter(gameObject.tag);
                    EnablePreview = true;
                    ClearTheRecord();
                    Destroy(gameObject);
                    GetComponentInParent<CharacterBar>().ModifyTheRoleBar(gameObject);//���½�ɫ��
                }
            }
        }

        protected override void EnableHighlighting()
        {
            if (targetObj.CompareTag("PlacablePoint"))
                targetObj.GetComponent<PlacablePoint>().IsBeSelected = true;
        }

        protected override void DisableHighlighting()
        {
            if (targetObj != null)
            {
                targetObj.GetComponent<PlacablePoint>().IsBeSelected = false;
                targetObj = null;
            }
        }

        protected override void DetectionObject()
        {
            base.DetectionObject();
            //�Ƿ�Ϊ�ɷ��ô�
            if(hitInfo.collider == null && targetObj != null)
            {
                DisableHighlighting();
            }
            else if(hitInfo.collider != null)
            {
                if (targetObj != hitInfo.collider.gameObject)
                {
                    DisableHighlighting();
                    targetObj = hitInfo.collider.gameObject;
                    EnableHighlighting();//�����ǰ�����Ķ���Ϊ�ɷ��õ㣬���ø���
                }
                
            }
        }

        /// <summary>
        /// �����ǰλ������ѡ�еĶ��󣬲���ȡ������ק������ռ���¼
        /// </summary>
        protected override void ClearTheRecord()
        {
            if(targetObj != null)
            {
                targetObj.GetComponent<PlacablePoint>().IsBeSelected = false;
                targetObj = null;
            }
        }



    }
}
