
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    /// <summary>
    /// ������������
    /// </summary>
    public class ItemBar : MonoBehaviour
    {
        //����
        private const float PosOffset = 145f;
        private const int MaximumQuantity = 5;

        //��ɫ��״̬
        [SerializeField] private int currentQuantity;
        [SerializeReference] private Transform startPoint;

        //��ɫ������
        [SerializeField] private List<GameObject> ItemList = new List<GameObject>();

        private void Start()
        {
            
        }

        
    }

}