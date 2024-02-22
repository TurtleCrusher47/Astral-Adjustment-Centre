using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    [SerializeField] protected MeleeWeaponData meleeWeaponData;
	[SerializeField] private Animator animator;
    
    private int attackStep = -1;
	protected Queue attackQueue = new();
	public Coroutine slashCoroutine = null;
    private float horizontalInput;
    private float verticalInput;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UsePrimary();
        }

		if (attackQueue.Count > 0 && slashCoroutine == null)
		{
			slashCoroutine = StartCoroutine(PlaySlash());
		}

        // To check if player tries to move during combo
        horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");
    }

    public void UpdateTransform()
    {
        // Read input buffer for chained slash attacks
		// If there are commands in queue but no slashes are playing,
		// start playing chained slash attacks
    }

    public void ClearSlashQueue()
    {
        attackQueue.Clear();
        attackStep = -1;
        animator.SetInteger("Attack", attackStep);
        slashCoroutine = null;
    }

    private IEnumerator PlaySlash()
	{
		// Remove last command in queue
		attackQueue.Dequeue();

		attackStep = (attackStep + 1) % 3;
		animator.SetInteger("Attack", attackStep);
		
		while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Slash " + attackStep))
		yield return null;

		// Wait for current slash animation to complete
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length - 0.2f);

		if (attackQueue.Count <= 0)
		{
			// Stop slashing when no more buttons in queue
			attackStep = -1;
			animator.SetInteger("Attack", attackStep);
			slashCoroutine = null;
		}

		// Else, continue to the next slash step
		else
		{
			if (horizontalInput != 0 || verticalInput != 0)
			{
				ClearSlashQueue();
			}
			else
			{
				slashCoroutine = StartCoroutine(PlaySlash());
			}
		}
	}
}
