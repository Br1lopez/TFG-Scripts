using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersOnTriggerEnter : Orders
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.HasComponent<PlayerAuto>())
        {
            RunOrders();
        }
    }

    IEnumerator WaitAndRun(float f)
    {
        yield return new WaitForSeconds(f);
        RunOrders();
    }
}
