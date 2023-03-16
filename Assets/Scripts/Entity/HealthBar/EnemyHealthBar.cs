using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Entity
{
    /// <summary>
    /// ����������Ѫ��
    /// </summary>
    public class EnemyHealthBar : MonoBehaviour
    {
        //UI
        private Image hpImage;
        private Image hpEffectImage;

        //����
        private float hp;
        private float maxHp;
        [SerializeField] private float hurtSpeed = 0.005f;

        private void Start()
        {
            InitHealthBar();//��ʼ��Ѫ��
        }

        private void InitHealthBar()
        {
            if (transform.GetChild(0).GetComponent<Image>() != null)
                hpImage = transform.GetChild(0).GetComponent<Image>();
            else
                Debug.LogError("δ�ҵ����");

            if (transform.GetChild(1).GetComponent<Image>() != null)
                hpEffectImage = transform.GetChild(1).GetComponent<Image>();
            else
                Debug.LogError("δ�ҵ����");

            if (GetComponentInParent<Enemy>() != null)
                maxHp = GetComponentInParent<Enemy>().MaxHealth;
            else
                Debug.LogError("δ�ҵ����");

            hp = maxHp;
        }

        private void Update()
        {
            hp = GetComponentInParent<Enemy>().Health;


            if (hp > 0)
                StartUpdateHealthBar();
        }

        private void StartUpdateHealthBar()
        {
            StartCoroutine(UpdateHealthBar());
        }

        //����Э��
        private IEnumerator UpdateHealthBar() 
        {
            hpImage.fillAmount = hp / maxHp;

            while(hpEffectImage.fillAmount >= hpImage.fillAmount)
            {
                hpEffectImage.fillAmount -= hurtSpeed;
                yield return new WaitForSeconds(0.005f);
            }

            if (hpEffectImage.fillAmount <= hpImage.fillAmount)
                hpEffectImage.fillAmount = hpImage.fillAmount;
        }


    }
}
