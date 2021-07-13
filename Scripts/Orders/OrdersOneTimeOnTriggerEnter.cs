using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersOneTimeOnTriggerEnter : Orders
{
    bool run = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!run&&collision.gameObject.HasComponent<PlayerAuto>())
        {
            RunOrders();
            run = true;
        }
    }

    IEnumerator WaitAndRun(float f)
    {
        yield return new WaitForSeconds(f);
        RunOrders();
    }
}
