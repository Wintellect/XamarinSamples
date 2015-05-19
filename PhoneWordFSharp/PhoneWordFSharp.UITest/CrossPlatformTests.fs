namespace PhoneWordFSharp.UITest

open Xamarin.UITest
open NUnit.Framework
open FsUnit

type ``Cross Platform Tests``() = 
    member val App = Option<IApp>.Some with get, set
