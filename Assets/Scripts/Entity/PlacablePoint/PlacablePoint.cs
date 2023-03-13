using Constant;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// 描述：中医师可放置点
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlacablePoint : MonoBehaviour
    {
        [Header("可放置处的状态")]
        [SerializeField] private bool isCanPlace;//是否可以放置
        [SerializeField] private bool isBeSelected;//鼠标是否悬浮在上面
        [SerializeField] private bool isThereAnyObstacle;//是否存在放置物
        [SerializeField] private GameObject placeObject;//放置的对象
        [SerializeField] private GameObject Obstacle;//阻碍物

        [Header("材质")]
        [SerializeField] private Material defaultMate;
        [SerializeField] private Material outlineMate;

        //组件
        private BoxCollider2D coll;
        private Renderer renderer2d;

        public bool IsBeSelected
        {
            set { isBeSelected = value; }
            get { return isBeSelected; }
        }
        public GameObject PlaceObject => placeObject;

        private void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            renderer2d = GetComponent<Renderer>();
            InitState();
        }

        private void Update()
        {
            if (isBeSelected)
            {
                //执行高亮
                renderer2d.material = outlineMate;
            }
            else
            {
                renderer2d.material = defaultMate;
            }


        }
        //公有方法//
        public void InitState()
        {
            isCanPlace = true;
            isBeSelected = false;
            isThereAnyObstacle = false;
            placeObject = null;
            coll.isTrigger = true;
            if(transform.childCount >0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }

        //放置中医师
        public void PlaceCharacter(string tag)
        {
            //如果当前是可以放置状态，并且
            if(isCanPlace)
            {
                GeneratCharacter(tag);
                isCanPlace = false;
            }
        }

        /// <summary>
        /// 当前是否在可放置处上，放置处是否有阻碍物
        /// 在放置处上，并且无阻碍物，为True
        /// 在放置处上，并且有阻碍物，为false
        /// 不在放置处上，为false
        /// </summary>
        /// <returns></returns>
        public bool CheckMouseOnTargetAndObstacle()
        {
            if (isBeSelected && !isThereAnyObstacle)
                return true;
            else if (isBeSelected && isThereAnyObstacle)
                return false;
            else
                return false;
        }

//私有方法//

        //生成中医师
        private void GeneratCharacter(string tag)
        {
            GameObject[] tcms = Resources.LoadAll<GameObject>(GameConst.TCMCharacter);

            foreach (var obj in tcms)
            {
                if(obj.CompareTag(tag))
                {
                    GameObject go =  Instantiate(obj,transform.position + Vector3.up, Quaternion.identity);
                    placeObject = go;
                    coll.enabled = false;
                    break;
                }
            }
        }
    }
}
