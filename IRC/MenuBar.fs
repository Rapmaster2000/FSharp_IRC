module MenuBar

open System
open System.Net
open System.IO
open System.Windows
open System.Windows.Forms
open System.Net.Sockets
open System.Drawing
open ConnectionDialog
open ServerCommunication

type UpdateFunc = IRCOp -> unit

[<AllowNullLiteral>]
type MenuBar =
    class
    inherit MenuStrip

    val mutable menu:ToolStripMenuItem
    val mutable aboutMenu:ToolStripMenuItem
    val mutable private subMenuConnect:ToolStripMenuItem
    val mutable private subMenuDisconnect:ToolStripMenuItem
    val mutable private subMenuBeenden:ToolStripMenuItem 
    [<DefaultValue>]
    val mutable private connection:IRCOp


    member private this.menuItemClicked (sender:obj) (eA:EventArgs) = 
        if sender :?> ToolStripMenuItem = this.subMenuConnect then
            use dialog = new ConnectionDialog () in
            dialog.ShowDialog this |> ignore
            match dialog.DialogResult with
            | DialogResult.OK -> let con = dialog.getCon in 
                                    try
                                    con.connect ()
                                    con.connectionSequence ()
                                    con.startRecvThread ()
                                    this.connection <- con
                                    this.subMenuDisconnect.Enabled <- true
                                    with 
                                    | :? SocketException | :? IOException as ex -> MessageBox.Show ((sprintf "Fehler beim Verbinden mit %s %i" con.getAdress con.getPort) , "Fehler") |> ignore
                                    | :? InvalidOperationException as ex -> MessageBox.Show("Fehler beim Verbinden" + Environment.NewLine, "Fehler") |> ignore
            | _ -> ()
        elif sender :?> ToolStripMenuItem = this.subMenuDisconnect then 
            this.connection.disconnect ()
            this.connection <- null
            this.subMenuDisconnect.Enabled <- false
        elif sender :?> ToolStripMenuItem = this.subMenuBeenden then
            Environment.Exit 1 |> ignore
        elif sender :?> ToolStripMenuItem = this.aboutMenu  then
            MessageBox.Show("IRC-Client written by F#-Soft.", "Über F#-IRC")  |> ignore



    member private this.init () =
        this.Location <- new Point(0,0)
        this.AutoSize <- true
        this.Name <- "TestTest"
        this.Text <- "TestTest"
        this.menu.Name <- "Server"
        this.menu.Text <- "Programm"

        this.aboutMenu.Name <- "About"
        this.aboutMenu.Text <- "Über"

        this.subMenuConnect.Name <- "ConnectButton"
        this.subMenuConnect.Text <- "Verbinden"

        this.subMenuBeenden.Name <- "ShutdownButton"
        this.subMenuBeenden.Text <- "Beenden"

        this.subMenuDisconnect.Name <- "DisconnectButton"
        this.subMenuDisconnect.Text  <- "Verbindung beenden"
        this.subMenuDisconnect.Enabled <- false

        this.Items.Add this.menu |> ignore
        this.Items.Add this.aboutMenu |> ignore
        let ev = new EventHandler(this.menuItemClicked) in
        this.aboutMenu.Click.AddHandler ev
        this.subMenuBeenden.Click.AddHandler ev
        this.subMenuConnect.Click.AddHandler ev
        this.subMenuDisconnect.Click.AddHandler ev
        this.menu.DropDownItems.Add this.subMenuConnect  |> ignore
        this.menu.DropDownItems.Add this.subMenuDisconnect  |> ignore
        this.menu.DropDownItems.Add this.subMenuBeenden  |> ignore

    new () as this = 
        {   
            menu = new ToolStripMenuItem(); 
            subMenuConnect = new ToolStripMenuItem(); 
            subMenuBeenden = new ToolStripMenuItem ();
            subMenuDisconnect = new ToolStripMenuItem();
            aboutMenu = new ToolStripMenuItem();
        }
        then
         this.init ()
    end
