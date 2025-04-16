using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectType1 : BombEffected
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
    /*    private void Awake()
        {
            
        }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log("Entered: " + collision.transform?.parent?.parent.gameObject);
        if ((targetLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            //Debug.Log("Triggered effectImpact");
            effectImpact(collision);
        }
    }

    public override void effectImpact(Collider2D collision)
    {
        explodeEffect.enabled = true;
        base.effectImpact(collision);
    }
}
