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

    val mutable public menu:ToolStripMenuItem
    val mutable private subMenuConnect:ToolStripMenuItem
    val mutable private subMenuBeenden:ToolStripMenuItem 
    [<DefaultValue>]
    val mutable private connection:IRCOp
    [<DefaultValue>]
    val mutable private updateConnection:UpdateFunc

    member public this.setUpdateFunc x = this.updateConnection <- x


    member private this.menuItemClicked (sender:obj) (eA:EventArgs) = 
        if sender :?> ToolStripMenuItem = this.subMenuConnect then
            let dialog = new ConnectionDialog () in
            dialog.ShowDialog this |> ignore
            match dialog.DialogResult with
            | DialogResult.OK -> let mutable con = dialog.getCon in 
                                 try
                                 //con.outputWindow <- this.textBox;
                                 con.connect (); 
                                 con.connectionSequence ();
                                 con.startRecvThread ();
                                 //TextBoxPanel.connection <- con;
                                 this.updateConnection con
                                 dialog.Dispose ()
                                 with 
                                 | :? SocketException | :? IOException as ex -> MessageBox.Show ((sprintf "Fehler beim Verbinden mit %s %i" con.getAdress con.getPort) , "Fehler") |> ignore; dialog.Dispose ();
                                 | :? InvalidOperationException as ex -> MessageBox.Show("Fehler beim Verbinden" + Environment.NewLine, "Fehler") |> ignore; dialog.Dispose ();
            | _ -> dialog.Dispose ()
        elif sender :?> ToolStripMenuItem = this.subMenuBeenden then
            Application.Exit () |> ignore
        //else MessageBox.Show("Unbekannter button") |> ignore


    member public this.init () =
        this.Location <- new Point(0,0)
        this.AutoSize <- true
        this.Name <- ""
        this.Text <- ""
        this.menu.Name <- "Server"
        this.menu.Text <- "Programm"
        this.subMenuConnect.Name <- "Verbinden"
        this.subMenuConnect.Text <- "Verbinden"
        this.subMenuBeenden.Name <- "Beenden"
        this.subMenuBeenden.Text <- "Beenden"
        this.Items.Add this.menu |> ignore
        this.subMenuBeenden.Click.AddHandler (new EventHandler(this.menuItemClicked))
        this.subMenuConnect.Click.AddHandler (new EventHandler(this.menuItemClicked))
        this.menu.DropDownItems.Add this.subMenuConnect  |> ignore
        this.menu.DropDownItems.Add this.subMenuBeenden  |> ignore

    new () as this = 
        { menu = new ToolStripMenuItem(); subMenuConnect = new ToolStripMenuItem(); subMenuBeenden = new ToolStripMenuItem ();}
        then
         this.init ()

    new updateFunc as this = 
        { menu = new ToolStripMenuItem(); subMenuConnect = new ToolStripMenuItem(); subMenuBeenden = new ToolStripMenuItem ();}
        then
         this.updateConnection <- updateFunc
         this.init ()
    end
