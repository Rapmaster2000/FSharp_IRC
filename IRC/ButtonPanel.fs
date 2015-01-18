module ButtonPanel

open System
open System.Windows
open System.Windows.Forms
open System.Drawing
open System.Collections.Generic
open ConnectionDialog



type ButtonPanel = 
    class
    inherit TableLayoutPanel
    val mutable private connectButton:Button
    val mutable private exitButton:Button

    member public this.init () = 
        this.ColumnCount <- 1
        this.RowCount <- 2

        this.connectButton.Text <- "Verbinden"
        this.exitButton.Text <- "Beenden"

        this.connectButton.Name <- "Verbinden"
        this.exitButton.Name <- "Beenden"
        (*
        this.exitButton.Click.AddHandler (fun obj x -> printfn "Exit Handler"; Aplication.Exit ())
        this.connectButton.Click.AddHandler (fun obj x -> printfn "Connecting... "; 
                                                          let d = new ConnectionDialog () in 
                                                          d.ShowDialog this |> ignore
                                                          match d.DialogResult with
                                                          | DialogResult.OK -> let con = d.getCon in 
                                                                                d.Dispose ()

                                                          | _ -> d.Dispose () ) *)
        this.Controls.Add (this.connectButton, 0, 0)
        this.Controls.Add (this.exitButton, 0, 1)

    member public this.getExitButton = this.exitButton
    member public this.getConnectButton = this.connectButton

    member public this.init (width, height) = 
        this.Height <- height
        this.Width <- width
        this.ColumnCount <- 1
        this.RowCount <- 2
        this.init ()

    new (width, height) as this =
        {exitButton = new Button(); connectButton = new Button ()}
        then 
        this.init (width, height)

    new () as this =
        {exitButton = new Button(); connectButton = new Button ()}
        then 
        this.init ()
     end
