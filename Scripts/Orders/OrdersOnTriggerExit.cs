using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersOnTriggerExit : Orders
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.HasComponent<PlayerAuto>())
        {
            RunOrders();
        }
    }
}
