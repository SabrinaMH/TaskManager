namespace HelloWorldApi

open Owin

open System
open System.Web.Http

type Startup() = 
    member this.Configuration(app : IAppBuilder) = 
        let apiConfig = new HttpConfiguration()
        apiConfig.MapHttpAttributeRoutes()
        app.UseWebApi(apiConfig) |> ignore