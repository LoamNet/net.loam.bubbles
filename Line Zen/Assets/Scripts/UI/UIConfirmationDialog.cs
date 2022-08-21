using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIConfirmationDialog : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] private CanvasGroup visibility;
    [SerializeField] private Button buttonConfirm;
    [SerializeField] private Button buttonCancel;

    // Internal Variables
    private Action onConfirm;
    private Action onCancel;
    public bool HasSomeVisibility { get; private set; }

#if UNITY_EDITOR
    // Editor only automatic validation
    private void OnValidate()
    {
        visibility = this.GetComponent<CanvasGroup>();
    }
#endif

    private void Awake()
    {
        SetVisibility(false);
    }

    private void SetVisibility(bool isVisible)
    {
        this.HasSomeVisibility = isVisible;
        visibility.alpha = isVisible ? 1 : 0;
        visibility.interactable = isVisible ? true : false;
        visibility.blocksRaycasts = isVisible ? true : false;

        if (isVisible)
        {
            gameObject.SetActive(true);
        }

        // Clear conditions when invisible to prevent issues
        if(isVisible == false)
        {
            onConfirm = null;
            onCancel = null;
        }
    }

    private void OnEnable()
    {
        buttonConfirm.onClick.AddListener(Confirm);
        buttonCancel.onClick.AddListener(Cancel);
    }

    private void OnDisable()
    {
        buttonConfirm.onClick.RemoveListener(Confirm);
        buttonCancel.onClick.RemoveListener(Cancel);
    }

    private void Confirm()
    {
        onConfirm?.Invoke();
        if(onConfirm == null)
        {
            Debug.LogError("Attempted to confirm, but no method was bound!");
        }
        SetVisibility(false);
    }

    private void Cancel()
    {
        onCancel?.Invoke();
        if (onCancel == null)
        {
            Debug.LogError("Attempted to cancel, but no method was bound!");
        }

        SetVisibility(false);
    }

    public void Display(Action onConfirm, Action onCancel)
    {
        this.onConfirm = onConfirm;
        this.onCancel = onCancel;

        SetVisibility(true);
    }
}
