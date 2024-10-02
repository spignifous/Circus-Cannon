using UnityEngine;

[CreateAssetMenu(fileName = "New InputReader", menuName = "Game/Data/Input Reader")]
public class InputReader : ScriptableObject
{
    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
        }

        _gameInput.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _gameInput.Gameplay.Disable();
    }

    public bool Touch => _gameInput.Gameplay.Touch.IsPressed();
    public bool TouchPressed => _gameInput.Gameplay.Touch.WasPressedThisFrame();
    public bool TouchReleased => _gameInput.Gameplay.Touch.WasReleasedThisFrame();

    public Vector2 TouchPosition => _gameInput.Gameplay.TouchPosition.ReadValue<Vector2>();


    /// <summary>
    /// Pegar a posição do toque na tela e converter para as coordenadas da cena
    /// </summary>
    public Vector3 TouchPositionWorld()
    {
        Vector3 mousePosition = TouchPosition;
        Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = Camera.main.nearClipPlane;
        return  Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
