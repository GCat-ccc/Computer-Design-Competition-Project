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
        [SerializeField] protected float health;//生命值
        [SerializeField] protected float maxHealth;//最大生命值
        [SerializeField] protected bool isCanAction = false;
        private float injuryInterval = 0.05f;//受伤间隔
        private float injuryTimer = 0f;//受伤计时器

        public float MaxHealth => maxHealth;
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
            injuryTimer -= Time.deltaTime;
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

        //被击中扣血
        public void Injured(float damage = 0, float deceleration = 1)
        {
            if (!isCanAction && injuryTimer < 0f) return;
            this.health -= damage;
            this.speed *= deceleration;//减速，以百分比减少0-0.5-1s
            injuryTimer = injuryInterval;

            if (health <= 0)
            {
                isCanAction = false;
                Destroy(gameObject);
            }
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
