module ServerCommunication

open System
open System.IO
open System.Net
open System.Text
open System.Threading
open System.Net.Sockets
open System.Collections.Generic
open System.Windows.Forms
open UpdateInterface

let bufSize = 2048

let convertToString buf =
    Encoding.ASCII.GetString buf

let convertToBuf (str: string) =
    Encoding.ASCII.GetBytes str 

type Delegate1 =  delegate of unit -> unit

[<AllowNullLiteral>]
type IRCOp = 
    class
    val mutable nick: string
    val mutable server: string
    val mutable port: int
    val mutable tcpCon: TcpClient
    val mutable channelList: List<string>
    [<DefaultValue>]
    val mutable public outputWindowNick:TextBox
    [<DefaultValue>]
    val mutable public outputWindow:TextBox 
    [<DefaultValue>]
    val mutable public thread:Thread
    [<DefaultValue>]
    val mutable private textBoxWindow:UpdateTextBox

    new(nick, server, port) = {nick = nick; server = server; port = port; tcpCon = new TcpClient() ; channelList = new List<string>() } 
    member this.connect () = 
        let ipA = Dns.GetHostAddresses this.server in
        match ipA with 
        | null | [||] -> printfn "No IPs found"
        | a -> for i in ipA do
                   i.ToString() |> printfn "Found address: %s" 
               this.tcpCon.Connect (a, this.port)
    
    member public this.setOutput a b =
        this.outputWindow <- a
        this.outputWindowNick <- b
        
    member public this.getAdress =
        this.server

    member public this.getPort =
        this.port


    member this.getMotD () = 
        let stream = this.tcpCon.GetStream () in
        let buf = Array.create bufSize 0uy
        let readBytes = ref 1 in 
        //stream.ReadTimeout <- 2000
        while !readBytes > 0 do
            try
            readBytes := stream.Read (buf, 0, buf.Length)
            if !readBytes > 0 then
                //this.outputWindow.AppendText (convertToString buf.[..readBytes - 1])
                this.textBoxWindow.updateTextBox (convertToString buf.[.. !readBytes - 1])
                //this.outputWindow.Invoke(new Delegate1(fun () -> this.outputWindow.AppendText (convertToString buf.[.. !readBytes - 1])  )) |> ignore
            with 
            | :? Exception as ex -> MessageBox.Show ("Message " + ex.Message) |> ignore

    member this.startRecvThread () =
        let thread = new Thread(this.getMotD) in
        this.thread <- thread
        thread.Start ()

    member this.stopThread () =
        if this.thread <> null then
            try
            this.thread.Abort ()
            with
            | :? Exception as ex -> MessageBox.Show ("Message " + ex.Message) |> ignore
        

    member this.genUser () = 
        let str = sprintf  "NICK %s\n\r" this.nick in
        convertToBuf str

    member this.genNick () =
        let str = sprintf "USER %s 0 * :Real unknown\n\r" "unknown" in 
        convertToBuf str

    member this.genJoin channel =
        let str = sprintf ":source JOIN :%s\n\r" channel in
        convertToBuf str

    member this.genPart channel =
        let str = sprintf ":source PART %s :reason 123 \n\r" channel in
        convertToBuf str

    member this.genQuit reason = 
        let str = sprintf ":source QUIT :reason %s\n\r" reason in
        convertToBuf str

    member this.connectionSequence () = 
        let stream = this.tcpCon.GetStream () in
        let buf = this.genNick () in 
        stream.Write (buf, 0, buf.Length)
        let buf2 = this.genUser () in 
        stream.Write (buf2, 0, buf2.Length)

    member this.setNick nick =
        this.nick <- nick
        let buf = this.genNick () in
        let stream = this.tcpCon.GetStream () in
        stream.Write (buf, 0, buf.Length)

    member this.joinChannel channel =
        this.channelList.Add(channel) 
        let buf = this.genJoin channel in
        let stream = this.tcpCon.GetStream () in
        stream.Write (buf, 0, buf.Length)

    member this.disconnect =
        let buf = this.genQuit "Goodbye" in 
        let stream = this.tcpCon.GetStream () in
        stream.Write (buf, 0, buf.Length)
        Thread.Sleep 3000
        this.tcpCon.Close

    override this.ToString () = 
        sprintf "Nick: %s\nServer: %s\nPort: %i\n"  this.nick this.server this.port
    end
