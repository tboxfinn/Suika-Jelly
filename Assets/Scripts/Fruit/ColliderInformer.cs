using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInformer : MonoBehaviour
{
    public bool WasCombinedIn;

    private bool _hasCollided;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_hasCollided && !WasCombinedIn)
        {
            _hasCollided = true;
            ThrowFruitController.instance.CanThrow = true;
            ThrowFruitController.instance.SpawnAFruit(FruitSelector.instance.NextFruit);
            FruitSelector.instance.PickNextFruit();
            Destroy(this);
        }
    }
}
