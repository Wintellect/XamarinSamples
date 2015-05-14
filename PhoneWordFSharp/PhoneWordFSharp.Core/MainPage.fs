namespace MainPage

open Xamarin.Forms
open PhoneWordFSharp.Core
open System.Collections.Generic
open System.Runtime.CompilerServices

[<Extension>]
type IListExtensions () =
    [<Extension>]
    static member inline AddRange(xs:'a IList, range) = range |> Seq.iter xs.Add

type IOpenUrlService =
    abstract member OpenUrl: string -> bool 

type MainPage() =
    static member GetMainPage() =
       let contentPage = ContentPage(Padding = Thickness(20., Device.OnPlatform(40., 20., 20.), 20., 20.))
       let panel = StackLayout(VerticalOptions = LayoutOptions.FillAndExpand, 
                               HorizontalOptions = LayoutOptions.FillAndExpand, 
                               Orientation = StackOrientation.Vertical, 
                               Spacing = 15.)
       let phoneNumberText = Entry(Text = "1-855-XAMARIN")
       let translateButton = Button(Text = "Translate")
       let callButton = Button(Text = "Call", IsEnabled = false)

       translateButton.Clicked.Add(fun _ -> callButton.Text <- PhoneTranslator.toNumber phoneNumberText.Text
                                            callButton.IsEnabled <- true)

       callButton.Clicked.Add(fun _ ->
           let isCalling = contentPage.DisplayAlert("Dial a number", "Would you like to call " + phoneNumberText.Text, "Yes", "No").Result

           if isCalling then
             let dialer = DependencyService.Get<IOpenUrlService>()
             dialer.OpenUrl phoneNumberText.Text |> ignore)

       panel.Children.AddRange([Label(Text = "Enter a Phoneword:")
                                phoneNumberText
                                translateButton
                                callButton])

       contentPage.Content <- panel
       contentPage

type App() =
    inherit Application(MainPage = MainPage.GetMainPage())