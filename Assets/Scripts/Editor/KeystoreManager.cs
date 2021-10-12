using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;

class MyCustomBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("MyCustomBuildProcessor.OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
        SetKeystore();
    }

    public static void SetKeystore()
    {
        string keystorePath = "Keystore/user.keystore";
        string keyaliasName = "1324224";
        string pass = "1324224";

        PlayerSettings.Android.keystoreName = keystorePath;
        PlayerSettings.Android.keyaliasName = keyaliasName;
        PlayerSettings.Android.keyaliasPass = pass;
        PlayerSettings.Android.keystorePass = pass;
    }
}