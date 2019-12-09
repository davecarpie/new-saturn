module NotesController

open Saturn

open FSharp.Control.Tasks.ContextInsensitive
open System.Net.Http
open Microsoft.AspNetCore.Http
open Giraffe.ResponseWriters

type Attachment = {
    name: string
    id: string
}

type Note = {
    content : string
    id : string
    attachments: Attachment list
}

let notes : Note list = [
    {
        content = "This is my first note"
        id = "note1"
        attachments = []
    };
    {
        content = "Second note second note"
        id = "note2"
        attachments = [{
            name = "image.jpg"
            id = "image1"
        }]
    }
]

let writeOtherUrlBack (url: string) (ctx: HttpContext) = task {
    let httpClient = new HttpClient()
    let! respString = httpClient.GetStringAsync(url)
    return! ctx.WriteTextAsync respString
} 

let writeOutIfSome ctx value = 
    match value with
    | Some v -> v |> Controller.json ctx
    | None -> "Not found" |> Response.notFound ctx


let showAttachment noteId = (fun ctx attachmentId ->    
    notes 
    |> List.tryFind (fun n -> n.id = noteId)
    |> Option.map (fun n -> n.attachments)
    |> Option.bind (List.tryFind (fun a -> a.id = attachmentId))
    |> writeOutIfSome ctx)

let allAttachments noteId = (fun ctx ->
    notes 
    |> List.tryFind (fun n -> n.id = noteId)
    |> Option.map (fun n -> n.attachments)                
    |> writeOutIfSome ctx)

let attachmentsController noteId = controller {
    show (showAttachment noteId)
    index (allAttachments noteId)
}

let showNote = (fun ctx id -> 
    notes 
    |> List.tryFind (fun n -> n.id = id)
    |> writeOutIfSome ctx)

let notesController = controller {
    subController "/attachments" attachmentsController

    index (fun ctx -> notes |> Controller.json ctx) 
    show showNote
}