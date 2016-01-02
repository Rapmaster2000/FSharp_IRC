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
    val mutable private adressField:TextBox
    val mutable private portField:TextBox
    [<DefaultValue>]
    val mutable private serverAdress:String
    [<DefaultValue>]
    val mutable private port:int
    [<DefaultValue>]
    static val mutable private instance:ConnectionDialog
    [<DefaultValue>]
    val mutable private connection:IRCOp 

    member this.getCon = this.connection

    member this.buttonClick  (sender:obj) (args:EventArgs) = 
        try
            let s = sender :?> Button in
            if s = this.okButton then
             this.serverAdress <- this.adressField.Text
             this.port <- Int32.Parse(this.portField.Text)
             let con = new IRCOp ("slash_Fsharp", this.serverAdress, this.port) in 
             this.connection <- con
             this.DialogResult <- DialogResult.OK
            else if s = this.cancelButton then
             this.DialogResult <- DialogResult.Cancel
        with 
        | :? FormatException as ex -> MessageBox.Show("Falsches Format!" + Environment.NewLine + "Füllen Sie die Felder korrekt aus!", "Fehler") |> ignore;
        | :? Exception as ex ->  MessageBox.Show("Fehler: " + ex.Message, "Fehler") |> ignore;

    member private this.init () = 
        this.Height <- 100
        this.Width <- 300
        this.StartPosition <- FormStartPosition.CenterScreen
        this.Text <- "Server eingeben"
        this.TopMost <- true
        //this.tableLayoutPanel.Height <- 100
        //this.tableLayoutPanel.Width <- 300
        this.tableLayoutPanel.AutoSize <- true
        let panel = new Panel () in
        let panel2 = new Panel () in

        panel.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle
        panel.Controls.Add this.adressField
        panel.Text <- "Server-Addresse"
        panel.AutoSize <- true
        this.okButton.AutoSize <- true

        panel2.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle
        panel2.Controls.Add this.portField
        panel2.Text <- "Port"
        panel2.AutoSize <- true

        this.cancelButton.Text <- "Abbrechen"
        this.okButton.Text <- "Verbinden"
        this.cancelButton.AutoSize <- true
        this.tableLayoutPanel.AutoSize <- true
        //this.cancelButton.Click.AddHandler (fun (sender:obj) (args:EventArgs) -> this.Dispose ())
        let eventHandler  = (new EventHandler(this.buttonClick)) in
        this.cancelButton.Click.AddHandler eventHandler
        this.okButton.Click.AddHandler eventHandler


        this.tableLayoutPanel.RowCount <- 2
        this.tableLayoutPanel.ColumnCount <- 2

        this.tableLayoutPanel.Controls.Add (panel, 0 ,0) 
        this.tableLayoutPanel.Controls.Add (panel2, 1, 0)
        this.tableLayoutPanel.Controls.Add (this.okButton, 0, 1)
        this.tableLayoutPanel.Controls.Add (this.cancelButton, 1, 1)
        this.Controls.Add this.tableLayoutPanel

    static member show () = 
        if ConnectionDialog.instance = null then
            ConnectionDialog.instance <- new ConnectionDialog ()
        //ConnectionDialog.instance.Visible <- true
        ConnectionDialog.instance.Show ()

    new () as this = 
       {tableLayoutPanel = new TableLayoutPanel (); okButton = new Button (); cancelButton = new Button (); adressField = new TextBox (); portField = new TextBox ()}
       then
       this.init ()
    end