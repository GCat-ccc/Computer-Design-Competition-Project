using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using UnityEngine.EventSystems;

namespace Entity
{
    /// <summary>
    /// 描述：角色卡牌即“塔”的卡牌道具，它UI层上，是被玩家拖拽到场景上使用的
    /// </summary>
    public class Character_Card :Card
    {

//重写父类的方法//
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
                    GetComponentInParent<CharacterBar>().ModifyTheRoleBar(gameObject);//更新角色栏
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
            //是否为可放置处
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
                    EnableHighlighting();//如果当前更换的对象为可放置点，启用高亮
                }
                
            }
        }

        /// <summary>
        /// 如果当前位置下有选中的对象，并且取消了拖拽，则清空检测记录
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
