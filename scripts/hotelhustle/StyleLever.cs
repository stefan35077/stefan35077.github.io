using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleLever : MonoBehaviour
{
    public Animator anim;
    HoldableObject holdableObject;

    private bool isOnCooldown = false;

    private void Start()
    {
        holdableObject = GetComponent<HoldableObject>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
      if(holdableObject.isGrabbed)
        {
            StartCoroutine(PullLever());
        } 
    }

    IEnumerator PullLever()
    {
        if (isOnCooldown)
            yield break;

        isOnCooldown = true;
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("PullLever");
        StyleManager.instance.ChangeObjectStyle();
        isOnCooldown = false;
    }
}
