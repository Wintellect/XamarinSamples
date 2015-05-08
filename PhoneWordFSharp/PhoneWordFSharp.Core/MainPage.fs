module MainPage

open Xamarin.Forms
open PhoneWordFSharp.Core

type IOpenUrlService =
    abstract member OpenUrl: string -> bool 

type MainPage() =
    static member GetMainPage =
       let contentPage = new ContentPage()
       let panel = new StackLayout()
       let phoneNumberText = new Entry()
       let translateButton = new Button()
       let callButton = new Button()
       let transator = PhoneTranslator()

       contentPage.Padding <- new Thickness(20., Device.OnPlatform(40., 20., 20.), 20., 20.)

       panel.VerticalOptions <- LayoutOptions.FillAndExpand
       panel.HorizontalOptions <- LayoutOptions.FillAndExpand
       panel.Orientation <- StackOrientation.Vertical
       panel.Spacing <- 15.

       phoneNumberText.Text <- "1-855-XAMARIN"
       translateButton.Text <- "Translate"
       callButton.Text <- "Call"
       callButton.IsEnabled <- false

       translateButton.Clicked.Add(fun _ -> 
            callButton.Text <- transator.toNumber phoneNumberText.Text
            callButton.IsEnabled <- true
        )

       callButton.Clicked.Add(fun _ ->
           let isCalling = contentPage.DisplayAlert("Dial a number", "Would you like to call " + phoneNumberText.Text, "Yes", "No")

           match isCalling.Result with
            | true -> let dialer = DependencyService.Get<IOpenUrlService>()
                      dialer.OpenUrl phoneNumberText.Text |> ignore
            | false -> ()
       )

       panel.Children.Add(new Label(Text = "Enter a Phoneword:"))
       panel.Children.Add(phoneNumberText)
       panel.Children.Add(translateButton)
       panel.Children.Add(callButton)

       contentPage.Content <- panel

       contentPage

type App() =
    inherit Application(MainPage = MainPage.GetMainPage)