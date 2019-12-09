module Server

open Saturn

let app = application {
    use_router Router.appRouter
    url "http://0.0.0.0:8085/"
}

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code