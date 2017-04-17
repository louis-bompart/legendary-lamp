using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Inventory
{
    internal interface IInventoryObserver
    {
        void OnInventoryChange();
    }
}
