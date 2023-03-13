using UnityEngine;


namespace Entity
{
    /// <summary>
    /// 描述：子弹类
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float flightSpeed;
        private Vector3 direction;
        private bool isSendOut = false;

        //子弹造成的效果
        private float damage;
        private float deceleration;

        private void Update()
        {
            if (isSendOut)
                transform.position += direction.normalized * flightSpeed * Time.deltaTime;
        }

        //调整方向，射出子弹
        public void Shoot(Vector3 dir,float damage = 0,float deceleration = 1)
        {
            direction = dir;
            this.damage = damage;
            this.deceleration = deceleration;
            isSendOut = true;
        }

        private void HitDetection()
        {
            Debug.Log("击中敌人");
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
                HitDetection();
        }

    }

}