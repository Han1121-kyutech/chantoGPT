using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 3.0f;        // 歩行速度（脱出ゲームは遅め）
    public float gravity = -9.81f;

    [Header("視点設定")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;        // 子カメラをInspectorでセット

    private CharacterController _cc;
    private Vector3 _velocity;
    private float _xRotation = 0f;        // カメラの上下回転量

    void Start()
    {
        _cc = GetComponent<CharacterController>();

        // カーソルをゲーム画面にロック
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleCursorToggle();
    }

    // ===== 視点（マウス）=====
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 上下：カメラだけ回転（-80〜80度でクランプ）
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // 左右：Playerオブジェクト全体を回転
        transform.Rotate(Vector3.up * mouseX);
    }

    // ===== 移動（WASD）=====
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S

        Vector3 move = transform.right * h + transform.forward * v;
        _cc.Move(move * moveSpeed * Time.deltaTime);

        // 重力
        if (_cc.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);
    }

    // ===== ESCでカーソル解放（UI操作のため）=====
    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        // クリックで再ロック
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}