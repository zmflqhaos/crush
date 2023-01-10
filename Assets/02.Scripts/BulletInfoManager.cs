using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletInfoManager : MonoSingleton<BulletInfoManager>
{
    [SerializeField] GameObject infopanel;
    [SerializeField] Image bulletImage;
    [SerializeField] Image skillImage;
    [SerializeField] Image hpBar;
    [SerializeField] Text nameText;
    [SerializeField] Text atkText;
    [SerializeField] Text hpText;
    public void ShowBulletInfo(Bullet bullet)
    {
        //알이 눌렸을 때 그 알의 정보를 표시한다.
        //카메라의 시점을 해당 알의 위치로 이동한다.
        infopanel.SetActive(true);
        hpBar.fillAmount = bullet.HP / bullet.MaxHP;
        nameText.text = bullet.name;
        atkText.text = "ATK : " + bullet.ATK;
        hpText.text = bullet.HP + " / " + bullet.MaxHP;
    }

    public void CloseBulletInfo()
    {
        //버튼을 제외한 다른 부분이 눌렸을 때 알의 정보를 닫는다.
        //카메라의 시점을 원 위치시키지는 않는다.
        infopanel.SetActive(false);
    }
}
