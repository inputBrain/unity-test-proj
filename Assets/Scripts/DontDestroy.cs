using Unity.VisualScripting;

public class DontDestroy : Singleton<DontDestroy>
{
    void Awake()
    {
        DontDestroyOnLoad(this.GameObject());
    }   
}
