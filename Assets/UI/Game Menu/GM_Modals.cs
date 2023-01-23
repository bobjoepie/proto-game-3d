using UnityEngine;
using UnityEngine.UIElements;

public class GM_Modals : VisualElement
{
    private VisualElement interactModal;
    private Label interactModalLabel;
    private VisualElement interactModalLabelOrigin;

    private VisualElement speechModal;
    private Label speechModalLabel;
    private VisualElement speechModalLabelOrigin;
    public new class UxmlFactory : UxmlFactory<GM_Modals, UxmlTraits> { }

    public GM_Modals()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        interactModal = this.Q("modal-interact-box");
        interactModalLabel = (Label)interactModal.Q("modal-label");
        interactModalLabelOrigin = this.Q("modal-interact-box-origin");

        speechModal = this.Q("modal-speech-box");
        speechModalLabel = (Label)speechModal.Q("modal-label");
        speechModalLabelOrigin = this.Q("modal-speech-box-origin");
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void ToggleInteractModal()
    {
        interactModal.ToggleInClassList("modal-interact-box");
        interactModal.ToggleInClassList("modal-interact-box-active");
    }

    public void ChangeInteractModalText(string text)
    {
        interactModalLabel.text = text;
    }

    public void MoveInteractModal(Vector2 position)
    {
        interactModalLabelOrigin.style.left = Length.Percent(position.x * 100f);
        interactModalLabelOrigin.style.top = StyleKeyword.Auto;
        interactModalLabelOrigin.style.right = StyleKeyword.Auto;
        interactModalLabelOrigin.style.bottom = Length.Percent(position.y * 100f);
    }

    public void ToggleSpeechModal()
    {
        speechModal.ToggleInClassList("modal-speech-box");
        speechModal.ToggleInClassList("modal-speech-box-active");
    }

    public void ChangeSpeechModalText(string text)
    {
        speechModalLabel.text = text;
    }

    public void MoveSpeechModal(Vector2 position)
    {
        speechModalLabelOrigin.style.left = Length.Percent(position.x * 100f);
        speechModalLabelOrigin.style.top = StyleKeyword.Auto;
        speechModalLabelOrigin.style.right = StyleKeyword.Auto;
        speechModalLabelOrigin.style.bottom = Length.Percent(position.y * 100f);
    }
}
