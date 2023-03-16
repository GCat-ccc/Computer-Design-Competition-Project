using UnityEngine;


namespace Entity
{
    /// <summary>
    /// �������ӵ���
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float flightSpeed;//�����ٶ�
        private Vector3 direction;//����
        private bool isSendOut = false;

        //�ӵ���ɵ�Ч��
        private float damage;
        private float deceleration;

        private void Update()
        {
            if (isSendOut)
                transform.position += direction.normalized * flightSpeed * Time.deltaTime;
        }

        //������������ӵ�
        public void Shoot(Vector3 dir,float damage = 0,float deceleration = 1)
        {
            direction = dir;
            this.damage = damage;
            this.deceleration = deceleration;
            isSendOut = true;
        }

        //��������
        private void HitDetection(GameObject obj)
        {
            Debug.Log("���е���");
            obj.GetComponent<Enemy>().Injured(damage,deceleration);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
                HitDetection(collision.gameObject);
        }

    }

}