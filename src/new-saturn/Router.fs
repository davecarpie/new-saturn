module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters

open NotesController

let appRouter = router {
    // forward "/api" apiRouter
    forward "/api" notesController
}