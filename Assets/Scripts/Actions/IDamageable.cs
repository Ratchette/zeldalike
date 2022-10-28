using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    public void TakeDamage(Vector2 force, float damage);
}

