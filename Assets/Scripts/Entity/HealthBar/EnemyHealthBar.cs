using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Entity
{
    /// <summary>
    /// 描述：敌人血条
    /// </summary>
    public class EnemyHealthBar : MonoBehaviour
    {
        //UI
        private Image hpImage;
        private Image hpEffectImage;

        //属性
        private float hp;
        private float maxHp;
        [SerializeField] private float hurtSpeed = 0.005f;

        private void Start()
        {
            InitHealthBar();//初始化血条
        }

        private void InitHealthBar()
        {
            if (transform.GetChild(0).GetComponent<Image>() != null)
                hpImage = transform.GetChild(0).GetComponent<Image>();
            else
                Debug.LogError("未找到组件");

            if (transform.GetChild(1).GetComponent<Image>() != null)
                hpEffectImage = transform.GetChild(1).GetComponent<Image>();
            else
                Debug.LogError("未找到组件");

            if (GetComponentInParent<Enemy>() != null)
                maxHp = GetComponentInParent<Enemy>().MaxHealth;
            else
                Debug.LogError("未找到组件");

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

        //缓冲协程
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
