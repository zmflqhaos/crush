using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBackGroundClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        BulletInfoManager.Instance.CloseBulletInfo();
    }
}
