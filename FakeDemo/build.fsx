#r "packages/FAKE/tools/FakeLib.dll"
#load "packageHelpers.fsx"
open Fake
open Fake.XamarinHelper

let buildDir = "FakeDemo/bin/Debug"
let testProj = !! "FakeDemo.UnitTests/FakeDemo.UnitTests.csproj"
let testDll = !! "FakeDemo.UnitTests/bin/Debug/FakeDemo.UnitTests.dll"
let uiTestProj = !! "UITests/FakeDemo.UITests.csproj"
let uiTestDll = !! "UITests/bin/Debug/FakeDemo.UITests.dll"

Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "Build-UnitTests" (fun _ ->
    testProj
        |> MSBuild "FakeDemo.UnitTests\bin\Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ]
        |> Log "---Unit Test build output----"
)

Target "Build-UITests" (fun _ ->
    uiTestProj
        |> MSBuild "UITests\bin\Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ]
        |> Log "---UI Test build output----"
)

Target "Run-UnitTests" (fun _ ->
    testDll |> NUnit ( fun defaults -> 
        { 
            defaults with ToolPath = "/Library/Frameworks/Mono.framework/Commands/"
                          ToolName = "nunit-console4" 
                          WorkingDir = "FakeDemo.UnitTests\bin\Debug"
                          DisableShadowCopy = true
        })
)

Target "Run-AndroidUITests" (fun _ ->
    let source = PackageHelpers.androidPackage()

    let dest = 
        filename source.FullName
        |> sprintf "./RebuyApp.Android.UITests/%s" 
        |> fileInfo
        |> PackageHelpers.moveAndroidApk source

    !! @"./**/RebuyApp.Android.UITests/bin/Release/RebuyApp.Android.UITests.dll"
        |> NUnit (fun defaults -> { defaults with ErrorLevel = DontFailBuild })

    Shell.Exec("adb", "uninstall de.rebuy.android")
        |> ignore

    DeleteFile dest.FullName
)

Target "Run-iOSUITests" (fun _ ->
    uiTestDll |> NUnit ( fun defaults -> 
        { 
            defaults with ToolPath = "/Library/Frameworks/Mono.framework/Commands/"
                          ToolName = "nunit-console4" 
        })
)

Target "Build-Pcl" (fun _ ->
    RestorePackages()

    !! "FakeDemo.csproj"
        |> MSBuild "FakeDemo/bin/Debug" "Build"  [ ("Configuration", "Debug"); ("Platform", "Any CPU") ] 
        |> Log "---PCL build output---"
)

Target "Build-iOS" (fun _ ->
    iOSBuild (fun defaults -> 
        { 
            defaults with ProjectPath = "iOS/FakeDemo.iOS.csproj"
                          OutputPath = "iOS/iPhoneSimulator/Debug"
                          Configuration = "Ad-Hoc|iPhone"
                          Target = "Build"
        })
)

Target "Build-Droid" (fun _ ->
    !! "Droid/FakeDemo.Droid.csproj"
        |> MSBuild "Droid/bin/Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ]
        |> Log "----Android build output----"
)

"Clean"
  ==> "Build-UnitTests"
  ==> "Run-UnitTests"

"Clean"
  ==> "Build-Pcl"
  ==> "Build-iOS"
  ==> "Build-Droid"
  ==> "Build-UITests"
  ==> "Run-AndroidUITests"

RunTargetOrDefault "Run-UnitTests"