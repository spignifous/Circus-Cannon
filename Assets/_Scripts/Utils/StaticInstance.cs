using Game.StateMachine;
using UnityEngine;

/// <summary>
/// Inst�ncia Est�tica
/// Util para redefinir o estado de uma inst�ncia
/// para evitar de fazer isso manualmente
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

public abstract class StateMachineStatic<T> : StateMachine where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}


/// <summary>
/// Isso transforma a inst�ncia est�tica em um singleton b�sico. Isto destruir� qualquer novas
/// vers�es criadas, deixando a inst�ncia original intacta
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance == null)
        {
            base.Awake();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
}

/// <summary>
/// Aqui temos uma vers�o persistente do singleton. Isso se manter� em todas as cenas
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}

