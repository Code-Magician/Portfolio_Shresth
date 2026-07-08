using TMPro;
using UnityEngine;

public class BuildInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text buildInfoText;

    private void Start()
    {
        if (BuildInfoLoader.Info == null)
        {
            buildInfoText.text = "No Build Info";
            return;
        }

        BuildInfo info = BuildInfoLoader.Info;

        buildInfoText.text =
$@"Version : {info.version}
Build   : {info.buildNumber}
Branch  : {info.branch}
Commit  : {info.commit}
Unity   : {info.unityVersion}
Workflow: {info.workflow}";
    }
}