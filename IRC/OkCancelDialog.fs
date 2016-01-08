module OkCancelDialog



open System.Windows.Forms
open System
open System.IO
open System.Windows
[<AllowNullLiteral>]
[<AbstractClass>]
type OkCancelDialog = 
    class
    inherit Form
    val private okButton:Button
    val private cancelButton:Button
    val private tableLayout:TableLayoutPanel
    val private buttonPanel:TableLayoutPanel
    [<DefaultValue>]
    val mutable private contentPanel:Panel

    new (panel) as this = 
        new OkCancelDialog ()
        then 
            this.addPanel panel

    new () as this = 
        {
         okButton = new Button ();
         cancelButton = new Button ();
         tableLayout = new TableLayoutPanel ();
         buttonPanel = new TableLayoutPanel ();
        }
        then
            this.AutoSize <- true
            this.okButton.Text <- "OK"
            this.cancelButton.Text <- "Abbrechen"
            this.buttonPanel.AutoSize <- true
            this.buttonPanel.RowCount <- 1
            this.buttonPanel.ColumnCount <- 2
            this.tableLayout.RowCount <- 2
            this.tableLayout.ColumnCount <- 1
            this.buttonPanel.Controls.Add (this.okButton, 0, 0)
            this.buttonPanel.Controls.Add (this.cancelButton, 1, 0)
            this.tableLayout.AutoSize <- true
            this.Controls.Add this.tableLayout
            this.tableLayout.Controls.Add (this.buttonPanel, 0, 1)
            let ev = new EventHandler(this.buttonClick) in
            this.okButton.Click.AddHandler ev
            this.cancelButton.Click.AddHandler ev

    member private this.buttonClick (sender:obj) (args:EventArgs) = 
        let button = sender :?> Button in
        if button = this.okButton then
            this.okClicked args
        elif button = this.cancelButton then
            this.cancelClicked args

    abstract okClicked: EventArgs -> unit

    abstract cancelClicked: EventArgs -> unit

    member this.addPanel panel =
        this.contentPanel <- panel
        this.tableLayout.Controls.Add (panel, 0, 0)
        this.Refresh ()

    end

