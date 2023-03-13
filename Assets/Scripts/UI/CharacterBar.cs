using System.Collections;
using Entity;
using Constant;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// ��������ɫ��
    /// </summary>
    public class CharacterBar : MonoBehaviour
    {
        //����
        private const float PosOffset = 145f;
        private const int MaximumQuantity = 5;

        //��ɫ��״̬
        [SerializeField] private int currentQuantity;
        [SerializeReference] private Transform startPoint;

        //��ɫ������
        [SerializeField] private List<GameObject> CharacterList = new List<GameObject>();

        private void Start()
        {
            InitCharacterBar();
        }

        public void InitCharacterBar()
        {
            GameObject[] characters = Resources.LoadAll<GameObject>(GameConst.TCMCard);
            if (characters == null) { Debug.LogError("������Դδ�ɹ�"); return; }

            for (int i = 0;i < MaximumQuantity; ++i)
            {
                if (startPoint == null) { Debug.LogError("δ�ҵ���ʼ��"); }
                var obj =  Instantiate(characters[i],startPoint.position+new Vector3(i*PosOffset,0,0),Quaternion.identity,transform);
                CharacterList.Add(obj);
                currentQuantity++;
            }
        }
//���з���//
        //�޸Ľ�ɫ����״̬
        public void ModifyTheRoleBar(GameObject go)
        {

            if (!CharacterList.Contains(go)) return;
            
            currentQuantity--;
            CharacterList.Remove(go);
            StartCoroutine(UpdateCharacterBar());
        }

//˽�з���//
        //���½�ɫ��
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