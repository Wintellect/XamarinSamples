#r "packages/FAKE/tools/FakeLib.dll"
open System.IO
open Fake
open Fake.XamarinHelper

let androidPackage() =
    AndroidPackage (fun defaults ->
        {
            defaults with ProjectPath = "Droid/FakeDemo.Droid.csproj"
        })

let moveAndroidApk (source : FileInfo) (dest : FileInfo) =
        DeleteFile dest.FullName
        Rename dest.FullName source.FullName

        dest
