#r "packages/FAKE/tools/FakeLib.dll"
#load "packageHelpers.fsx"
open Fake
open Fake.XamarinHelper

let buildDir = "FakeDemo/bin/Debug"
let testProj = !! "FakeDemo.UnitTests/FakeDemo.UnitTests.csproj"
let testDll = !! "FakeDemo.UnitTests/bin/Debug/FakeDemo.UnitTests.dll"

Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "Build-UnitTests" (fun _ ->
    testProj
        |> MSBuild "FakeDemo.UnitTests\bin\Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ]
        |> Log "---Unit Test build output----"
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
  ==> "Run-UnitTests"

RunTargetOrDefault "Run-UnitTests"