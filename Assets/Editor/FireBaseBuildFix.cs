using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
public class FireBaseBuildFix
{
    [PostProcessBuild]
    public static void OnPostProcessBuildAddFirebaseFile(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // Go get pbxproj file
            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

            // PBXProject class represents a project build settings file,
            // here is how to read that in.
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);

            // Copy plist from the project folder to the build folder
            proj.AddFileToBuild(proj.GetUnityMainTargetGuid(), proj.AddFile("GoogleService-Info.plist", "GoogleService-Info.plist"));

            // Write PBXProject object back to the file
            proj.WriteToFile(projPath);
        }
    }

    const string GameKitFrameworkName = "GameKit.framework";
    [PostProcessBuild(101)]
    private static void PostProcessBuild_LinkGameKit(BuildTarget target, string buildPath)
    {
#if UNITY_EDITOR_OSX
        if (target == BuildTarget.iOS)
        {

            string projPath = PBXProject.GetPBXProjectPath(buildPath);
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            // embed gamekit into Unity framework target because its used in the facebooksdk
            Debug.Log("Embedding gamekit frameworks into UnityFramework target...");
            string targetGuid = proj.GetUnityFrameworkTargetGuid();

            proj.AddFrameworkToProject(targetGuid, GameKitFrameworkName, true);
            proj.WriteToFile(projPath);
        }
#endif
    }
}
