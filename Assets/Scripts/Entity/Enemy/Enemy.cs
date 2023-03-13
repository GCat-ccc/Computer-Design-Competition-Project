using DG.Tweening;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// 敌人类的基类，即疾病/病毒
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [Header("敌人的基本属性")]
        [SerializeField] protected float speed;
        [SerializeField] protected float health;
        [SerializeField] protected bool isCanAction = false;

        public float Health => health;

        [Header("移动线路")]
        [SerializeField] private Transform[] waypoints;
        private int currentWaypointIndex;//当前路径点
        private bool isArriveTargetPoint;

        private void Start()
        {
            
        }

        private void Update()
        {
            if(isCanAction)
                GoToTarget();
        }

//公有方法//

        //初始化敌人的属性
        public void InitEnemyProperty(Transform[] waypoints)
        {
            this.waypoints = waypoints;
            currentWaypointIndex = 0;
            isCanAction = true;
        }


//私有方法//
        
        //敌人按固定路线移动逻辑
        private void GoToTarget()
        {
            Vector3 targetPos = waypoints[currentWaypointIndex].position;
            Vector3 direction = targetPos - transform.position;

            float distance = direction.magnitude;
            if (distance < 0.1f)
            {
                currentWaypointIndex++;
                HasEnemyArrivedAtDestination();//判断敌人是否到达了终点
            }
            else
            {
                transform.position += direction.normalized * speed * Time.deltaTime;
            }
        }

        //检测敌人是否到达了终点
        private void HasEnemyArrivedAtDestination()
        {
            if(currentWaypointIndex == waypoints.Length)
            {
                isCanAction = false;
                isArriveTargetPoint = true;
                //触发敌人到达终点事件，更新记录敌人到达数量的UI
            }
        }

    }
}
