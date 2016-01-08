module UpdateInterface

open System
open System.Net
open System.IO


type UpdateTextBox =
    abstract updateNickBox: string -> unit
    abstract updateTextBox: string -> unit



