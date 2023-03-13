using System.Collections;
using Entity;
using Constant;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// 描述：角色栏
    /// </summary>
    public class CharacterBar : MonoBehaviour
    {
        //常量
        private const float PosOffset = 145f;
        private const int MaximumQuantity = 5;

        //角色栏状态
        [SerializeField] private int currentQuantity;
        [SerializeReference] private Transform startPoint;

        //角色栏容器
        [SerializeField] private List<GameObject> CharacterList = new List<GameObject>();

        private void Start()
        {
            InitCharacterBar();
        }

        public void InitCharacterBar()
        {
            GameObject[] characters = Resources.LoadAll<GameObject>(GameConst.TCMCard);
            if (characters == null) { Debug.LogError("加载资源未成功"); return; }

            for (int i = 0;i < MaximumQuantity; ++i)
            {
                if (startPoint == null) { Debug.LogError("未找到起始点"); }
                var obj =  Instantiate(characters[i],startPoint.position+new Vector3(i*PosOffset,0,0),Quaternion.identity,transform);
                CharacterList.Add(obj);
                currentQuantity++;
            }
        }
//公有方法//
        //修改角色栏的状态
        public void ModifyTheRoleBar(GameObject go)
        {

            if (!CharacterList.Contains(go)) return;
            
            currentQuantity--;
            CharacterList.Remove(go);
            StartCoroutine(UpdateCharacterBar());
        }

//私有方法//
        //更新角色栏
        private IEnumerator UpdateCharacterBar()
        {
            for(int i = 0; i < CharacterList.Count; ++i)
            {
                CharacterList[i].transform.DOMoveX(startPoint.position.x +
                    i * PosOffset, 0.1f);
                yield return new WaitForSeconds(0.1f);
                CharacterList[i].GetComponent<Card>().SaveTheCardStatus();

            }
        }
    }

}