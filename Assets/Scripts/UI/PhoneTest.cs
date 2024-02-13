using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTest : MonoBehaviour
{
    public Animator animator;

    public void OpenPhone()
    {
        animator.SetBool("PhoneClicked", true);
    }

    public void ClosePhone()
    {
        animator.SetBool("PhoneClicked", false); 
    }
}
