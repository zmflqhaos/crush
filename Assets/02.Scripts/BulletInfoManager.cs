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
        //���� ������ �� �� ���� ������ ǥ���Ѵ�.
        //ī�޶��� ������ �ش� ���� ��ġ�� �̵��Ѵ�.
        infopanel.SetActive(true);
        hpBar.fillAmount = bullet.HP / bullet.MaxHP;
        nameText.text = bullet.name;
        atkText.text = "ATK : " + bullet.ATK;
        hpText.text = bullet.HP + " / " + bullet.MaxHP;
    }

    public void CloseBulletInfo()
    {
        //��ư�� ������ �ٸ� �κ��� ������ �� ���� ������ �ݴ´�.
        //ī�޶��� ������ �� ��ġ��Ű���� �ʴ´�.
        infopanel.SetActive(false);
    }
}
