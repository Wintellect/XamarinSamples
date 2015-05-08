module OpenUrl

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget
open System.Linq
open Xamarin.Forms
open MainPage
open Android.Net

type OpenUrlService() =
    let isIntentAvailable (context:Context) (intent:Intent) = 
        let packageManager = context.PackageManager
        let list = packageManager.QueryIntentServices(intent, PM.PackageInfoFlags.Services).Union(packageManager.QueryIntentActivities(intent, PM.PackageInfoFlags.Activities))

        if list.Any() then true
        else false

    interface IOpenUrlService with
        member this.OpenUrl url = 
            let context = Forms.Context
            let intent = new Intent(Intent.ActionCall)
            
            match context = null with
            | true -> ()
            | false -> intent.SetData(Uri.Parse("tel:" + url)) |> ignore

            isIntentAvailable context intent