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
open ObserverListener.Observer
open ObserverListener.Listener
open IRCMessage

let bufSize = 2048

let convertToString buf =
    Encoding.ASCII.GetString buf

let convertToBuf (str: string) =
    Encoding.ASCII.GetBytes str 

type Delegate1 =  delegate of unit -> unit

[<AllowNullLiteral>]
type IRCOp = 
    class
    interface Observer with 
        member this.registerListener l = this.listeners.Add(l)
        member this.removeListener l = 
            if this.listeners.Contains(l) then 
                this.listeners.Remove(l) |> ignore

    val mutable nick: string
    val mutable server: string
    val mutable port: int
    val mutable tcpCon: TcpClient
    val mutable channelList: List<string>
    [<DefaultValue>]
    val mutable outputWindowNick:TextBox
    [<DefaultValue>]
    val mutable outputWindow:TextBox 
    [<DefaultValue>]
    val mutable thread:Thread
    [<DefaultValue>]
    val mutable private textBoxWindow:UpdateTextBox
    [<DefaultValue>]
    val mutable private listeners: List<Listener<IRCMessage>>

    new(nick, server, port) = 
        {
        nick = nick;
        server = server;
        port = port;
        tcpCon = new TcpClient();
        channelList = new List<string>();
        } 

    member this.connect () = 
        let ipA = Dns.GetHostAddresses this.server in
        match ipA with 
        | null | [||] -> printfn "No IPs found"
        | a -> for i in ipA do
                   printfn "Found address: %s" (i.ToString ());
               this.tcpCon.Connect (a, this.port)
    
    member this.setOutput a b =
        this.outputWindow <- a
        this.outputWindowNick <- b
        
    member this.getAdress =
        this.server

    member this.getPort =
        this.port

    member this.getMotD () = 
        let stream = this.tcpCon.GetStream () in
        let buf = Array.create bufSize 0uy in 
        let readBytes = ref 1 in 
        //stream.ReadTimeout <- 2000
        while !readBytes > 0 do
            try
            readBytes := stream.Read (buf, 0, buf.Length)
            if !readBytes > 0 then
                Console.WriteLine readBytes
                //this.outputWindow.AppendText (convertToString buf.[..readBytes - 1])
//                this.textBoxWindow.updateTextBox (convertToString buf.[.. !readBytes - 1])
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
            this.thread <- null
            with
            | :? ThreadAbortException as ex -> MessageBox.Show ("Message " + ex.Message, "Exception") |> ignore
        

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
        //this.connect ()
        let stream = this.tcpCon.GetStream () in
        let buf = this.genNick () in 
        stream.Write (buf, 0, buf.Length)
        let buf2 = this.genUser () in 
        stream.Write (buf2, 0, buf2.Length)
        (*this.getMotD*)

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

    member this.disconnect () =
        let buf = this.genQuit "123" in 
        if this.tcpCon.Connected then 
            let stream = this.tcpCon.GetStream () in
            stream.Write (buf, 0, buf.Length)
            Thread.Sleep 3000
            this.tcpCon.Close ()

    override this.ToString () = 
        sprintf "Nick: %s\nServer: %s\nPort: %i\n"  this.nick this.server this.port
    end
