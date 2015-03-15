module ConnectionDialog

open System
open System.Windows
open System.Windows.Forms
open System.Drawing
open System.Collections.Generic
open ServerCommunication

[<AllowNullLiteral>]
type ConnectionDialog = 
    class
    inherit Form
    val mutable private tableLayoutPanel:TableLayoutPanel
    val mutable private okButton:Button 
    val mutable private cancelButton:Button
    val mutable private addressField:TextBox
    val mutable private portField:TextBox
    [<DefaultValue>]
    val mutable private serverAddress:String
    [<DefaultValue>]
    val mutable private port:int
    [<DefaultValue>]
    static val mutable private instance:ConnectionDialog
    [<DefaultValue>]
    val mutable private connection:IRCOp 

    member public this.getCon = this.connection

    member private this.buttonClick  (sender:obj) (args:EventArgs) = 
        try
            let s = sender :?> Button in
            if s = this.okButton then
             this.serverAddress <- this.addressField.Text
             this.port <- Int32.Parse(this.portField.Text)
             let con = new IRCOp ("slash_Fsharp", this.serverAddress, this.port) in 
             this.connection <- con
             this.DialogResult <- DialogResult.OK
            else if s = this.cancelButton then
             this.DialogResult <- DialogResult.Cancel
        with 
        | :? FormatException as ex -> MessageBox.Show("Falsches Format!" + Environment.NewLine + "Füllen Sie die Felder korrekt aus!", "Fehler") |> ignore;
        | :? Exception as ex ->  MessageBox.Show("Fehler: " + ex.Message, "Fehler") |> ignore;

    member private this.init () = 
        let groupBox = new GroupBox (Text = "Server-Adresse", AutoSize = true) in
        let groupBox2 = new GroupBox(Text = "Port", AutoSize = true) in
        //this.Height <- 100
        //this.Width <- 300
        this.AutoSize <- true
        this.StartPosition <- FormStartPosition.CenterScreen
        this.Text <- "Server eingeben"
        this.TopMost <- true
        //this.FormBorderStyle <- FormBorderStyle.FixedSingle
        //this.tableLayoutPanel.Height <- 100
        //this.tableLayoutPanel.Width <- 300
        this.tableLayoutPanel.Dock <- DockStyle.Fill
        this.tableLayoutPanel.AutoSize <- true
        let panel = new Panel () in
        let panel2 = new Panel () in
        this.addressField.Location <- new Point(10, 20)
        groupBox.Controls.Add  this.addressField

        this.portField.Location <- new Point (10, 20)
        groupBox2.Controls.Add this.portField

        //panel.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle
       // panel.Controls.Add groupBox
        //panel.Text <- "Server-Adresse"
        panel.AutoSize <- true
        //panel.Controls.Add groupBox
        this.okButton.AutoSize <- true
        this.tableLayoutPanel.Controls.Add (groupBox, 0 ,0) 
        this.tableLayoutPanel.Controls.Add (groupBox2, 1 ,0) 
        this.tableLayoutPanel.Controls.Add (this.okButton, 0, 1)
        this.tableLayoutPanel.Controls.Add (this.cancelButton, 1, 1)
        let eventHandler  = (new EventHandler(this.buttonClick)) in
        this.cancelButton.Click.AddHandler eventHandler
        this.okButton.Click.AddHandler eventHandler
        (*panel2.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle *)
        this.Controls.Add this.tableLayoutPanel

    static member public show () = 
        if ConnectionDialog.instance = null then
            ConnectionDialog.instance <- new ConnectionDialog ()
        //ConnectionDialog.instance.Visible <- true
        ConnectionDialog.instance.Show ()

    new () as this = 
       {tableLayoutPanel = new TableLayoutPanel (); okButton = new Button (Text = "Verbinden" ); cancelButton = new Button (Text="Abbrechen"); addressField = new TextBox (); portField = new TextBox ()}
       then
       this.init ()
    end