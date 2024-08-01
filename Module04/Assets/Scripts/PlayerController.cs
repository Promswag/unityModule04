using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rigidBody;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private AudioSource audioSource;

	private bool isJumping = false;
	private bool isGrounded = true;
	private float horizontalVelocity = 0f;

	[SerializeField] private float speed = 1f;
	[SerializeField] private float jumpForce = 1f;

	private int maxHp = 3;
	private int hp = 3;

	private bool isDead;
	// private bool isMoving;

	[SerializeField] private AudioClip jumpAudioClip;
	[SerializeField] private AudioClip takesDamageAudioClip;
	[SerializeField] private AudioClip diesAudioClip;
	[SerializeField] private AudioClip respawnsAudioClip;

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
        horizontalVelocity = Input.GetAxis("Horizontal") * speed;

		if (isJumping == false) 
		{
			if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
			{
				isJumping = true;
				isGrounded = false;
				animator.SetTrigger("hasJumped");
				rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
				PlayAudioClip(jumpAudioClip);
			}
		}

		Vector2 move = transform.TransformDirection(new Vector2(horizontalVelocity, rigidBody.velocity.y));
		rigidBody.velocity = new Vector3(move.x, rigidBody.velocity.y);

		animator.SetFloat("velocity", Mathf.Abs(rigidBody.velocity.x));
		
		FlipX();
		// if (!isMoving)
		// {
		// 	if (Input.GetKeyDown(KeyCode.J))
		// 	{
		// 		isMoving = true;
		// 		spriteRenderer.flipX = false;
		// 		animator.Play("Base Layer.Walk2");
		// 	}
		// 	else if (Input.GetKeyDown(KeyCode.G))
		// 	{
		// 		isMoving = true;
		// 		spriteRenderer.flipX = true;
		// 		animator.Play("Base Layer.Walk2");
		// 	}
		// }
	}

	void FlipX()
	{
		if (rigidBody.velocity.x < -0.5f)
			spriteRenderer.flipX = true;
		if (rigidBody.velocity.x > 0.5f)
			spriteRenderer.flipX = false;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		foreach (ContactPoint2D contact in collision.contacts)
		{
			if (contact.normal == Vector2.up)
			{
				isGrounded = true;
				if (isJumping)
				{
					isJumping = false;
					animator.SetTrigger("hasLanded");
				}
			}
		}
	}

	public void TakesDamage(int amount)
	{
		if (isDead)
			return;

		hp -= amount;
		if (hp > 0)
		{
			animator.SetTrigger("takesDamage");
			Debug.LogFormat("The caterpillar is hit!\n{0} HP remaining!", hp);
			PlayAudioClip(takesDamageAudioClip);
		}
		else
		{
			isDead = true;
			animator.SetTrigger("dies");
			Debug.Log("The caterpillar has died!");
			StartCoroutine(Respawn());
			PlayAudioClip(diesAudioClip);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
			isGrounded = false;
	}

	IEnumerator Respawn()
	{
		GameManager.instance.Respawn();
		enabled = false;
		yield return new WaitForSeconds(2.9f);
		spriteRenderer.flipX = false;
		animator.SetTrigger("respawns");
		PlayAudioClip(respawnsAudioClip);
		yield return new WaitForSeconds(1.2f);
		hp = maxHp;
		enabled = true;
		isDead = false;
	}

	void PlayAudioClip(AudioClip clip)
	{
		audioSource.clip = clip;
		audioSource.Play();
	}

	// public void MoveStep()
	// {
	// 	isMoving = false;
	// 	transform.position = new(transform.position.x + (spriteRenderer.flipX ? -3.3f : 3.3f), transform.position.y);
	// }
}
