namespace HelloWorldApi

open System
open System.Web.Http

type HomeController() = 
    inherit ApiController()

    [<HttpGet>]
    [<Route("hello")>]
    member this.Hello() =
        "Hello World!"