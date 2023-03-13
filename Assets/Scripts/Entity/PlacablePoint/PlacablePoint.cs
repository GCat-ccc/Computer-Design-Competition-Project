using Constant;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// ��������ҽʦ�ɷ��õ�
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlacablePoint : MonoBehaviour
    {
        [Header("�ɷ��ô���״̬")]
        [SerializeField] private bool isCanPlace;//�Ƿ���Է���
        [SerializeField] private bool isBeSelected;//����Ƿ�����������
        [SerializeField] private bool isThereAnyObstacle;//�Ƿ���ڷ�����
        [SerializeField] private GameObject placeObject;//���õĶ���
        [SerializeField] private GameObject Obstacle;//�谭��

        [Header("����")]
        [SerializeField] private Material defaultMate;
        [SerializeField] private Material outlineMate;

        //���
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
                //ִ�и���
                renderer2d.material = outlineMate;
            }
            else
            {
                renderer2d.material = defaultMate;
            }


        }
        //���з���//
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

        //������ҽʦ
        public void PlaceCharacter(string tag)
        {
            //�����ǰ�ǿ��Է���״̬������
            if(isCanPlace)
            {
                GeneratCharacter(tag);
                isCanPlace = false;
            }
        }

        /// <summary>
        /// ��ǰ�Ƿ��ڿɷ��ô��ϣ����ô��Ƿ����谭��
        /// �ڷ��ô��ϣ��������谭�ΪTrue
        /// �ڷ��ô��ϣ��������谭�Ϊfalse
        /// ���ڷ��ô��ϣ�Ϊfalse
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

//˽�з���//

        //������ҽʦ
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
