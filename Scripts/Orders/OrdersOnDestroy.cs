using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersOnDestroy : Orders
{
    private void OnDestroy()
    {
        RunOrders();
    }
}
