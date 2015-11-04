#r "packages/FAKE/tools/FakeLib.dll"
open System.IO
open Fake
open Fake.XamarinHelper

let androidPackage() =
    AndroidPackage (fun defaults ->
        {
            defaults with ProjectPath = "Droid/FakeDemo.Droid.csproj"
        })

let movePackageFile (source : FileInfo) (dest : FileInfo) =
        DeleteFile dest.FullName
        Rename dest.FullName source.FullName

        dest

let generateIPA() = 
        iOSBuild (fun defaults -> 
        { 
            defaults with ProjectPath = "iOS/FakeDemo.iOS.csproj"
                          OutputPath = "iOS/iPhone/Release"
                          Configuration = "Ad-Hoc|iPhone"
                          Target = "Build"
        })
