using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploseKnockback : BombEffected
{
    public PointEffector2D explodeEffect;

    protected  void Awake()
    {
        if (explodeEffect == null)
        {
            explodeEffect = GetComponent<PointEffector2D>();
            explodeEffect.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("here");
        if ((targetLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            effectImpact(collision);
            Debug.Log("here");
        }
    }

    public override void effectImpact(Collider2D collision)
    {
        explodeEffect.enabled = true;
        base.effectImpact(collision);
    }
}
