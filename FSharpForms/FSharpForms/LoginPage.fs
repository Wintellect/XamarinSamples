namespace FSharpForms

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml

type UserData = {
    mutable Username: string
    mutable Password: string
}

type LoginPage() as self =
    inherit ContentPage()

    let userData = { Username = "test"; Password = "test" }

    do base.LoadFromXaml(typeof<LoginPage>) |> ignore

    let usernameEntry = base.FindByName<Entry>("usernameEntry")
    let passwordEntry = base.FindByName<Entry>("passwordEntry")
    let loginButton = base.FindByName<Button>("loginButton")

    do usernameEntry.TextChanged.Add(fun e -> userData.Username <- e.NewTextValue )
    do passwordEntry.TextChanged.Add(fun e -> userData.Password <- e.NewTextValue )

    do loginButton.Clicked.Add(fun e -> self.DisplayAlert("Data", "User: " + userData.Username + " Pass: " + userData.Password, "OK") |> ignore)