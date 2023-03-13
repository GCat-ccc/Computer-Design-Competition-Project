using System.Collections.Generic;
using Constant;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// 描述：中医师类
    /// </summary>
    public class Character : MonoBehaviour
    {
        //中医师的属性
        [SerializeField]protected float damage;//伤害
        [SerializeField]protected float attackSpeed;//攻击速度
        [SerializeField]protected float deceleration;//使敌人减速
        protected float timer = 0f;//距离下一次攻击的剩余时间
        private GameObject bulletPrefab;

        //供外部调用
        public float Damage => damage;
        public float AttackSpeed => attackSpeed;

        //中医师的状态
        private GameObject currentTarget;

        //在范围内的攻击目标
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


//公有方法//


        //提升角色的属性值
        public void PromoteCharacterAttributes(float damage,float attackSpeed)
        {
            if(damage < 0||attackSpeed < 0) { Debug.LogError("传入的值错误");return; }

            this.damage += damage;
            this.attackSpeed += attackSpeed;
        }


//私有方法//

       
        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.CompareTag("Enemy"))
            {
                //将进入范围内的攻击对象，加入List
                if (!attackTargetList.Contains(collision.gameObject))
                    attackTargetList.Add(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //如果敌人还在List中，当他离开了攻击范围则移除
            if (attackTargetList.Contains(collision.gameObject))
                attackTargetList.Remove(collision.gameObject);
        }

        //处理攻击逻辑
        private void AttackLogic()
        {
            if(timer < 0f && attackTargetList.Count > 0)
            {
                Debug.Log("执行攻击逻辑");
                DrawDownTheEnemyByBlood();//索敌

                TransformOrientation();//转换朝向

                AttackTarget();//攻击目标

                timer = 2f / attackSpeed;
            }
        }

        //攻击目标
        private void AttackTarget()
        {
            GameObject bullet = Resources.Load<GameObject>(GameConst.TCMBullet);//可使用对象池优化

            //计算方向
            Vector3 dir = currentTarget.transform.position - transform.position;
            GameObject go = Instantiate(bullet, transform.position,bullet.transform.rotation);
            go.GetComponent<Bullet>().Shoot(dir,damage,deceleration);
        }

        //转向
        private void TransformOrientation()
        {
            if (currentTarget.transform.localScale.x > transform.localScale.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }

        //索敌
        private void DrawDownTheEnemyByBlood()
        {
            //排序出血量最少的敌人
            float minHealth = 10000f;
            int index = 0;

            for(int i = 0; i < attackTargetList.Count; ++i)
            {
                if (attackTargetList[i] == null)
                {
                    Debug.LogError("空引用" + attackTargetList[i]);
                    return;
                }

                if(attackTargetList[i].GetComponent<Enemy>().Health < minHealth)
                {
                    minHealth = attackTargetList[i].GetComponent<Enemy>().Health;
                    index = i;
                }
            }

            currentTarget = attackTargetList[index];//将血量最少的敌人作为目标
        }

        
        
    }
}
