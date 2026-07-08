using UnityEngine;

public class BuildInfoTester : MonoBehaviour
{
    void Start()
    {
        BuildInfo info = BuildInfo.Load();

        if (info != null)
        {
            Debug.Log(JsonUtility.ToJson(info, true));
        }
    }
}