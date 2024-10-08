using UnityEngine;

namespace Game.Systems
{
    public class Bootstrapper : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute() => DontDestroyOnLoad(Instantiate(Resources.Load("Systems")));
    }
}