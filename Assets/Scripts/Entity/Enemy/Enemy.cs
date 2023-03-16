using DG.Tweening;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// ������Ļ��࣬������/����
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [Header("���˵Ļ�������")]
        [SerializeField] protected float speed;
        [SerializeField] protected float health;//����ֵ
        [SerializeField] protected float maxHealth;//�������ֵ
        [SerializeField] protected bool isCanAction = false;
        private float injuryInterval = 0.05f;//���˼��
        private float injuryTimer = 0f;//���˼�ʱ��

        public float MaxHealth => maxHealth;
        public float Health => health;

        [Header("�ƶ���·")]
        [SerializeField] private Transform[] waypoints;
        private int currentWaypointIndex;//��ǰ·����
        private bool isArriveTargetPoint;

        private void Start()
        {
            
        }

        private void Update()
        {
            injuryTimer -= Time.deltaTime;
            if(isCanAction)
                GoToTarget();
        }

//���з���//

        //��ʼ�����˵�����
        public void InitEnemyProperty(Transform[] waypoints)
        {
            this.waypoints = waypoints;
            currentWaypointIndex = 0;
            isCanAction = true;
        }

        //�����п�Ѫ
        public void Injured(float damage = 0, float deceleration = 1)
        {
            if (!isCanAction && injuryTimer < 0f) return;
            this.health -= damage;
            this.speed *= deceleration;//���٣��԰ٷֱȼ���0-0.5-1s
            injuryTimer = injuryInterval;

            if (health <= 0)
            {
                isCanAction = false;
                Destroy(gameObject);
            }
        }

//˽�з���//
        
        //���˰��̶�·���ƶ��߼�
        private void GoToTarget()
        {
            Vector3 targetPos = waypoints[currentWaypointIndex].position;
            Vector3 direction = targetPos - transform.position;

            float distance = direction.magnitude;
            if (distance < 0.1f)
            {
                currentWaypointIndex++;
                HasEnemyArrivedAtDestination();//�жϵ����Ƿ񵽴����յ�
            }
            else
            {
                transform.position += direction.normalized * speed * Time.deltaTime;
            }
        }

        //�������Ƿ񵽴����յ�
        private void HasEnemyArrivedAtDestination()
        {
            if(currentWaypointIndex == waypoints.Length)
            {
                isCanAction = false;
                isArriveTargetPoint = true;
                //�������˵����յ��¼������¼�¼���˵���������UI
            }
        }

    }
}
