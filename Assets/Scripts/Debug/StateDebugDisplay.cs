using TMPro;
using UnityEngine;

public class StateDebugDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI label;

    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, 0f);

    private IStateMachineProvider provider;
    private IState lastState;
    private Transform canvasTransform;
    private Vector3 canvasBaseScale;
    private float lastSignX = 1f;

    private void Awake()
    {
        provider = GetComponent<IStateMachineProvider>();

        if (label != null && label.canvas != null)
        {
            canvasTransform = label.canvas.transform;
        }
    }

    private void Start()
    {
        if (canvasTransform != null)
        {
            canvasTransform.localPosition = offset;
            canvasBaseScale = canvasTransform.localScale;
        }
    }

    private void LateUpdate()
    {
        CorrectCanvasScale();
        UpdateStateLabel();
    }

    private void CorrectCanvasScale()
    {
        if (canvasTransform == null)
            return;

        float signX = Mathf.Sign(transform.localScale.x);

        if (signX == lastSignX)
            return;

        lastSignX = signX;

        canvasTransform.localScale = new Vector3(
            Mathf.Abs(canvasBaseScale.x) * signX,
            canvasBaseScale.y,
            canvasBaseScale.z);
    }

    private void UpdateStateLabel()
    {
        if (provider == null || provider.StateMachine == null)
            return;

        IState current = provider.StateMachine.CurrentState;

        if (current == lastState)
            return;

        lastState = current;

        if (label != null)
            label.text = StateDisplayNames.Get(current);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.Label(
            transform.position + offset,
            "StateDebugDisplay");
    }
#endif
}
