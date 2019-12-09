module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters

open NotesController
open System.Text
open Microsoft.AspNetCore.Http


let text (str : string) : HttpHandler =
    let bytes = Encoding.UTF8.GetBytes str
    fun (_ : HttpFunc) (ctx : HttpContext) ->
        ctx.SetContentType "text/plain; charset=utf-8"
        ctx.WriteBytesAsync bytes

let writeback (_: HttpFunc) = writeOtherUrlBack "http://google.com"
let appRouter = router {
    // forward "/api" apiRouter

    forward "/api" notesController
    get "/google" writeback //(writeOtherUrlBack "http://www.google.com")
}