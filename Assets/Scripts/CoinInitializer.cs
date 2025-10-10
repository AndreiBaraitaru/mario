using UnityEngine;

public class CoinInitializer : MonoBehaviour
{
    void Awake()
    {
        var gm = GameObject.FindGameObjectWithTag("Manager")?.GetComponent<GameManager>();
        var tool = GetComponent<AnimationEventIntTool>();

        if (gm != null && tool != null)
        {
            tool.useInt.AddListener(gm.IncreaseScore);
        }
    }
}
