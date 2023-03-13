
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    /// <summary>
    /// ÃèÊö£ºµÀ¾ßÀ¸
    /// </summary>
    public class ItemBar : MonoBehaviour
    {
        //³£Á¿
        private const float PosOffset = 145f;
        private const int MaximumQuantity = 5;

        //½ÇÉ«À¸×´Ì¬
        [SerializeField] private int currentQuantity;
        [SerializeReference] private Transform startPoint;

        //½ÇÉ«À¸ÈÝÆ÷
        [SerializeField] private List<GameObject> ItemList = new List<GameObject>();

        private void Start()
        {
            
        }

        
    }

}