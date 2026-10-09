using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ComboController : MonoBehaviour
{
    [Header("Configuración")]
    public float comboTimeout = 1.0f;

    [Header("Fuerzas")]
    public float jumpForce = 8f;
    public float sideForce = 6f;
    public float torqueForce = 15f;

    private Rigidbody rb;
    private List<KeyCode> inputBuffer = new List<KeyCode>();
    private float timer = 0f;

    private readonly KeyCode[] comboLeftToRight = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.Q, KeyCode.A };
    private readonly KeyCode[] comboVertical    = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Q, KeyCode.A };
    private readonly KeyCode[] comboRightToLeft = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.UpArrow };

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Control de tiempo entre pulsaciones
        if (inputBuffer.Count > 0)
        {
            timer += Time.deltaTime;
            if (timer > comboTimeout)
            {
                ResetBuffer();
            }
        }

        // Detección de teclas 
        DetectKey(KeyCode.UpArrow);
        DetectKey(KeyCode.DownArrow);
        DetectKey(KeyCode.Q);
        DetectKey(KeyCode.A);
    }

    void DetectKey(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            CancelInvoke(nameof(ExecuteArcLeftWithReset));
            inputBuffer.Add(key);
            timer = 0f;
            CheckCombos();
        }
    }

    void CheckCombos()
    {
        // Comprobar combos
        if (MatchesSequence(comboLeftToRight))
        {
            ExecuteArcRight();
            ResetBuffer();
        }
        else if (MatchesSequence(comboVertical))
        {
            ExecuteVerticalJump();
            ResetBuffer();
        }
        else if (MatchesSequence(comboRightToLeft))
        {
            Invoke(nameof(ExecuteArcLeftWithReset), 0.3f);
        }
    }

    bool MatchesSequence(KeyCode[] combo)
    {
        if (inputBuffer.Count != combo.Length) return false;

        for (int i = 0; i < combo.Length; i++)
        {
            if (inputBuffer[i] != combo[i]) return false;
        }
        return true;
    }

    void ResetBuffer()
    {
        inputBuffer.Clear();
        timer = 0f;
    }

    // Combo 1
    void ExecuteArcRight()
    {
        rb.linearVelocity = Vector3.zero;
        Vector3 force = (Vector3.right * sideForce) + (Vector3.up * jumpForce);
        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(Vector3.back * torqueForce, ForceMode.Impulse);
    }

    // Combo 2
    void ExecuteVerticalJump()
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * (jumpForce * 1.5f), ForceMode.Impulse);
    }

    // Combo 3
    void ExecuteArcLeft()
    {
        rb.linearVelocity = Vector3.zero;
        Vector3 force = (Vector3.left * sideForce) + (Vector3.up * jumpForce);
        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(Vector3.forward * torqueForce, ForceMode.Impulse);
    }

    void ExecuteArcLeftWithReset()
    {
        ExecuteArcLeft();
        ResetBuffer();
    }
}
