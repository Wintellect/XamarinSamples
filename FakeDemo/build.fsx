#r "packages/FAKE/tools/FakeLib.dll"
open Fake

Target "Test" (fun _ ->
    trace "Testing stuff..."
)

Target "Build" (fun _ ->
    trace "Heavy build action"
)

"Build"            // define the dependencies
   ==> "Test"

Run "Test"