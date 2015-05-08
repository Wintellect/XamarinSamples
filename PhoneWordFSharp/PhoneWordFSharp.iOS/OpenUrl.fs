module OpenUrl

open UIKit
open Foundation
open MainPage

type OpenUrlService() =
    interface IOpenUrlService with
        member this.OpenUrl url = UIApplication.SharedApplication.OpenUrl(new NSUrl("tel:" + url))