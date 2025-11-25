using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    public Animator animator;

    public KeyCode keyToTrigger = KeyCode.F;
    public string triggerToPlay = "FlyBool";

    private void OnValidate()
    {
        if(animator != null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToTrigger))
        {
            animator.SetBool(triggerToPlay, !animator.GetBool(triggerToPlay));
        }
    }
}