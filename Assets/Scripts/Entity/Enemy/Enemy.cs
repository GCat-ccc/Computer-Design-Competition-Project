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
        [SerializeField] protected float health;
        [SerializeField] protected bool isCanAction = false;

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
