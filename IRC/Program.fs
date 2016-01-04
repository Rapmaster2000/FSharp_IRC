// Weitere Informationen zu F# unter "http://fsharp.net".
// Weitere Hilfe finden Sie im Projekt "F#-Lernprogramm".
module Program

open System
open System.Net
open System.Text
open System.Threading
open System.Net.Sockets
open System.Collections.Generic

open ServerCommunication
open GUI



[<EntryPoint>]
[<STAThread>]
let main argv = 
    try
        let a = new IRCOp("slash_Fsharp", "wilhelm.freenode.net", 6667) in
        let instance = GUIManager.getInstance () in
        let nickList = [|"Hallo"; "1234"; "foobar";"25gab"|] in
        //instance.addPage "1. Seite"
        //instance.updateNickBox nickList
        instance.run ()
        //a.connectionSequence ()
        //a.joinChannel "#pcsx2"
        printfn "Conncted to %s" (a.ToString ())
        (*ignore(Console.ReadKey ()) *)
        printfn "Disconnecting..."
        a.disconnect () 
        (*ignore(Console.ReadKey ()) *)
    with 
        | :? NullReferenceException as ex -> printfn "Null-Pointer exception: %s" ex.Message;
        | :? SocketException as ex -> printfn "Failed to connect: %s"  ex.Message;
        | :? Exception as ex -> printfn "Caught exception: %s"  ex.Message;
    0 // Exitcode aus ganzen Zahlen zurückgeben
