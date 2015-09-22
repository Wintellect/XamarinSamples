#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.XamarinHelper

let buildDir = "FakeDemo/bin/Debug"

Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "Test" (fun _ ->
    trace "Testing stuff..."
)

Target "BuildPcl" (fun _ ->
    RestorePackages ()

    !! "FakeDemo.csproj"
        |> MSBuild "FakeDemo/bin/Debug" "Build"  [ ("Configuration", "Debug"); ("Platform", "Any CPU") ] 
        |> Log "---PCL build output---"
)

Target "Build-iOS" (fun _ ->
    iOSBuild (fun defaults -> 
        { 
            defaults with ProjectPath = "iOS/FakeDemo.iOS.csproj"
                          OutputPath = "iOS/iPhoneSimulator/Debug"
                          Configuration = "Debug|iPhoneSimulator"
                          Target = "Build"
        })
)

Target "Build-Droid" (fun _ ->
    !! "Droid/FakeDemo.Droid.csproj"
        |> MSBuild "Droid/bin/Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ]
        |> Log "----Android build output----"
)

"BuildPcl"
   ==> "Clean"
   ==> "Test"

"Build-Droid"
   ==> "Build-iOS"

RunTargetOrDefault "Test"