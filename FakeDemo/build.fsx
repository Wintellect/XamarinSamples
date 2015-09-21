#r "packages/FAKE/tools/FakeLib.dll" // 1
open Fake // 2

Target "Test" (fun _ -> // 3
    trace "Testing stuff..."
)

Target "Build" (fun _ ->
    trace "Heavy build action"
)

"Build" // 4
   ==> "Test"

Run "Test" // 5