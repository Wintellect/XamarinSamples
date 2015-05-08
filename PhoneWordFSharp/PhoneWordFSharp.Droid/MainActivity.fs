namespace PhoneWordFSharp.Droid

open System
open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget
open Xamarin.Forms
open MainPage
open OpenUrl
open Xamarin.Forms.Platform.Android

[<assembly: Dependency(typeof<OpenUrlService>)>] do ()

[<Activity (Label = "PhoneWordFSharp.Droid", MainLauncher = true)>]
type MainActivity () =
    inherit FormsApplicationActivity ()

    override this.OnCreate (bundle) =
        base.OnCreate (bundle)

        Forms.Init(this, bundle)

        this.LoadApplication(App())