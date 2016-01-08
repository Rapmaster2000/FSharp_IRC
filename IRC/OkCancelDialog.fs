module OkCancelDialog



open System.Windows.Forms
open System
open System.IO
open System.Windows
[<AbstractClass>]
type OkCancelDialog = 
    class
    inherit Form
    val private okButton:Button
    val private cancelButton:Button
    val private tableLayout:TableLayoutPanel
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
        }
        then
            this.AutoSize <- true
            this.tableLayout.RowCount <- 2
            this.tableLayout.ColumnCount <- 1
            this.Controls.Add this.tableLayout
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

