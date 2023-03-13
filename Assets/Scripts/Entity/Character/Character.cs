using System.Collections.Generic;
using Constant;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// ��������ҽʦ��
    /// </summary>
    public class Character : MonoBehaviour
    {
        //��ҽʦ������
        [SerializeField]protected float damage;//�˺�
        [SerializeField]protected float attackSpeed;//�����ٶ�
        [SerializeField]protected float deceleration;//ʹ���˼���
        protected float timer = 0f;//������һ�ι�����ʣ��ʱ��
        private GameObject bulletPrefab;

        //���ⲿ����
        public float Damage => damage;
        public float AttackSpeed => attackSpeed;

        //��ҽʦ��״̬
        private GameObject currentTarget;

        //�ڷ�Χ�ڵĹ���Ŀ��
        [SerializeField] private List<GameObject> attackTargetList = new List<GameObject>();

        private void Start()
        {
            currentTarget = null;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            AttackLogic();
        }


//���з���//


        //������ɫ������ֵ
        public void PromoteCharacterAttributes(float damage,float attackSpeed)
        {
            if(damage < 0||attackSpeed < 0) { Debug.LogError("�����ֵ����");return; }

            this.damage += damage;
            this.attackSpeed += attackSpeed;
        }


//˽�з���//

       
        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.CompareTag("Enemy"))
            {
                //�����뷶Χ�ڵĹ������󣬼���List
                if (!attackTargetList.Contains(collision.gameObject))
                    attackTargetList.Add(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //������˻���List�У������뿪�˹�����Χ���Ƴ�
            if (attackTargetList.Contains(collision.gameObject))
                attackTargetList.Remove(collision.gameObject);
        }

        //�������߼�
        private void AttackLogic()
        {
            if(timer < 0f && attackTargetList.Count > 0)
            {
                Debug.Log("ִ�й����߼�");
                DrawDownTheEnemyByBlood();//����

                TransformOrientation();//ת������

                AttackTarget();//����Ŀ��

                timer = 2f / attackSpeed;
            }
        }

        //����Ŀ��
        private void AttackTarget()
        {
            GameObject bullet = Resources.Load<GameObject>(GameConst.TCMBullet);//��ʹ�ö�����Ż�

            //���㷽��
            Vector3 dir = currentTarget.transform.position - transform.position;
            GameObject go = Instantiate(bullet, transform.position,bullet.transform.rotation);
            go.GetComponent<Bullet>().Shoot(dir,damage,deceleration);
        }

        //ת��
        private void TransformOrientation()
        {
            if (currentTarget.transform.localScale.x > transform.localScale.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }

        //����
        private void DrawDownTheEnemyByBlood()
        {
            //�����Ѫ�����ٵĵ���
            float minHealth = 10000f;
            int index = 0;

            for(int i = 0; i < attackTargetList.Count; ++i)
            {
                if (attackTargetList[i] == null)
                {
                    Debug.LogError("������" + attackTargetList[i]);
                    return;
                }

                if(attackTargetList[i].GetComponent<Enemy>().Health < minHealth)
                {
                    minHealth = attackTargetList[i].GetComponent<Enemy>().Health;
                    index = i;
                }
            }

            currentTarget = attackTargetList[index];//��Ѫ�����ٵĵ�����ΪĿ��
        }

        
        
    }
}
