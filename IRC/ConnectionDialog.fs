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
        | :? FormatException | :? ArgumentNullException as ex -> MessageBox.Show("Falsches Format!" + Environment.NewLine + "Füllen Sie die Felder korrekt aus!", "Fehler") |> ignore;
        | :? OverflowException as ex ->  MessageBox.Show("Fehler: " + ex.Message, "Fehler") |> ignore;

    member private this.init () = 
//        this.Height <- 130
//        this.Width <- 250 
        this.StartPosition <- FormStartPosition.CenterScreen
        this.Text <- "Verbinden"
        this.TopMost <- true
        this.FormBorderStyle <- FormBorderStyle.FixedSingle
        this.tableLayoutPanel.AutoSize <- true
        let panel = new Panel () in
        let panel2 = new Panel () in

        panel.BorderStyle <- BorderStyle.None
        this.adressField.Anchor <- (AnchorStyles.Left  + AnchorStyles.Top)
        this.adressField.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle
        panel.Controls.Add this.adressField
        panel.Text <- "Server-Addresse"
        panel.AutoSize <- true
        this.okButton.AutoSize <- true

        panel2.BorderStyle <- BorderStyle.None
        this.portField.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle
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


        this.tableLayoutPanel.RowCount <- 3
        this.tableLayoutPanel.ColumnCount <- 2

        let addressLabel = new Label () in 
        let portLabel = new Label ()
        addressLabel.Text <- "Server-Adresse:"
        portLabel.Text <-  "Port:"
        addressLabel.Anchor <- (AnchorStyles.Left + AnchorStyles.Top)
        this.tableLayoutPanel.Controls.Add(addressLabel, 0, 0)
        this.tableLayoutPanel.Controls.Add (panel, 1 ,0) 
        this.tableLayoutPanel.Controls.Add (portLabel, 0, 1)
        this.tableLayoutPanel.Controls.Add (panel2, 1, 1)
        this.tableLayoutPanel.Controls.Add (this.okButton, 0, 2)
        this.tableLayoutPanel.Controls.Add (this.cancelButton, 1, 2)
        this.Controls.Add this.tableLayoutPanel

    static member show () = 
        if ConnectionDialog.instance = null then
            ConnectionDialog.instance <- new ConnectionDialog ()
        ConnectionDialog.instance.Show ()

    static member destroy () =
        ConnectionDialog.instance <- null

    new () as this = 
       {
        tableLayoutPanel = new TableLayoutPanel (); 
        okButton = new Button (); 
        cancelButton = new Button (); 
        adressField = new TextBox (); 
        portField = new TextBox ()
        }
       then
       this.init ()
    end