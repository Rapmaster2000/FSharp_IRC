module ConnectionDialog

open System
open System.Windows
open System.Windows.Forms
open System.Drawing
open System.Collections.Generic
open ServerCommunication
open OkCancelDialog

[<AllowNullLiteral>]
type ConnectionDialog = 
    class
    val private tableLayoutPanel:TableLayoutPanel
    val private addressField:TextBox
    val private portField:TextBox
    [<DefaultValue>]
    val mutable private serverAddress:String
    [<DefaultValue>]
    val mutable private port:int
    [<DefaultValue>]
    val mutable private connection:IRCOp 

    member this.getCon = this.connection

    member private this.buttonClick  (sender:obj) (args:EventArgs) = ()
//        try
//            let s = sender :?> Button in
//            if s = this.okButton then
//                this.serverAddress <- this.addressField.Text
//                this.port <- Int32.Parse(this.portField.Text)
//                let con = new IRCOp ("slash_Fsharp", this.serverAddress, this.port) in 
//                this.connection <- con
//                this.DialogResult <- DialogResult.OK
//            else if s = this.cancelButton then
//             this.DialogResult <- DialogResult.Cancel
//        with 
//        | :? FormatException | :? ArgumentNullException as ex -> MessageBox.Show("Falsches Format!" + Environment.NewLine + "Füllen Sie die Felder korrekt aus!", "Fehler") |> ignore;
//        | :? OverflowException as ex ->  MessageBox.Show("Fehler: " + ex.Message, "Fehler") |> ignore;

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
        this.addressField.Anchor <- (AnchorStyles.Left  + AnchorStyles.Top)
        this.addressField.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle
        panel.Controls.Add this.addressField
        panel.Text <- "Server-Adresse"
        panel.AutoSize <- true

        panel2.BorderStyle <- BorderStyle.None
        this.portField.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle
        panel2.Controls.Add this.portField
        panel2.Text <- "Port"
        panel2.AutoSize <- true

        //panel.BorderStyle <- System.Windows.Forms.BorderStyle.FixedSingle

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
        this.addPanel this.tableLayoutPanel

    inherit OkCancelDialog 
    override this.okClicked ev  = () 
    override this.cancelClicked ev =  this.Dispose ()
        


    new () as this = 
       {
        tableLayoutPanel = new TableLayoutPanel (); 
        addressField = new TextBox (); 
        portField = new TextBox ()
       }
       then
        this.init ()
    end