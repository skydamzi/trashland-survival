using UnityEngine;
public class Player : MonoBehaviour
{
    public VariableJoystick joystick;
    
    public float moveSpeed = 3f;
    public float tiltAngle = 10f;
    public float tiltSpeed = 30f;
    Vector2 lastNonZeroMoveInput = Vector2.down;
    public SpriteRenderer player_sr;
    public Rigidbody2D player_rb;
    public Animator player_ani;

    void Awake()
    {
        player_rb = GetComponent<Rigidbody2D>();
        player_ani = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        Vector2 moveInput = new Vector2(moveX, moveY);
        Vector2 moveDirection = moveInput.normalized;

        player_rb.linearVelocity = moveDirection * moveSpeed;
        bool isMoving = moveInput.magnitude > 0.1f;

        if (isMoving) lastNonZeroMoveInput = moveInput; // 마지막으로 입력된 방향을 저장

        float maxTilt = tiltAngle;
        Vector2 moveDir = moveInput.normalized;
        float tiltPercent = Mathf.Abs(moveDir.x);
        float targetZ = -moveDir.x * maxTilt * tiltPercent;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZ);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * tiltSpeed);

        player_ani.SetBool("isRunning", isMoving);

        if (isMoving)
        {
            player_ani.SetFloat("InputX", moveInput.x);
            player_ani.SetFloat("InputY", moveInput.y);
        }
        else // 움직이지 않을 때는 마지막으로 입력된 방향
        {
            player_ani.SetFloat("InputX", lastNonZeroMoveInput.x);
            player_ani.SetFloat("InputY", lastNonZeroMoveInput.y);
        }
    }
}