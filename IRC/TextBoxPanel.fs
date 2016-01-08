module TextBoxPanel

open System
open System.IO
open System.Windows
open System.Windows.Forms
open System.Net.Sockets
open System.Drawing
open System.Windows.Forms
open System.Collections.Generic
open ButtonPanel
open ConnectionDialog
open ServerCommunication
open Parser
open UpdateInterface


let windowWidth = 800
let windowHeight = 400

type TabFunction = string -> unit
type TabCloseFunction = unit -> unit


type TextBoxPanel = 
    class
    inherit TableLayoutPanel 
    val private textBox:TextBox
    val private nickList:TextBox
    val private inputBox:TextBox
    [<DefaultValue>]
    val mutable private tabControl:TabControl
    [<DefaultValue>]
    val mutable private addTab:TabFunction
    [<DefaultValue>]
    static val mutable private connection:IRCOp


    member this.closeTab =
        let tab = this.tabControl.SelectedTab in
        if this.tabControl.TabCount > 1 then
           tab.Dispose () |> ignore
        

    member this.textBoxHandler (source:obj) (x:EventArgs) = 
        let box = source :?> TextBox in 
        if box = this.inputBox then
            let text = box.Text in
                if text.Contains Environment.NewLine then
                 box.Clear ()
                 if TextBoxPanel.connection <> null then
                    begin
                     if text.[0] = '/' then
                      match text with
                      | JOIN -> let pa = getParams text in
                                if pa <> null then
                                    this.addTab pa.[0]
                                    TextBoxPanel.connection.joinChannel pa.[0]
                      | PART ->   this.closeTab 
                      | LIST ->    MessageBox.Show("list command")  |> ignore
                      | QUIT ->    MessageBox.Show("quit command")  |> ignore
                      | INVALID -> MessageBox.Show("Nothing found") |> ignore
                //Send command
                     else this.textBox.AppendText text
                    end

    member this.init width height=
        let offset = 100 in 
        this.Height <- height
        this.Width <- width 
        this.Dock <- DockStyle.Top

        this.Location <- Point (0, 0)
        this.ColumnCount <- 2
        this.RowCount <- 2

        this.nickList.BorderStyle <- BorderStyle.FixedSingle
        this.inputBox.BorderStyle <- BorderStyle.FixedSingle
        this.textBox.BorderStyle <- BorderStyle.FixedSingle

        this.textBox.Multiline <- true
        this.nickList.Multiline  <- true
        this.inputBox.Multiline <- true

        this.textBox.AutoSize <- true
        this.nickList.AutoSize <- true
        this.inputBox.AutoSize <- true

        this.textBox.Height <- this.Height - offset
        this.textBox.Width <- this.Width - offset
        this.textBox.ScrollBars <- ScrollBars.Vertical
        this.textBox.BackColor <- Color.White

        this.nickList.Height <- this.Height - offset
        this.nickList.Width <- offset
        this.nickList.ScrollBars <- ScrollBars.Vertical
        this.nickList.BackColor <- Color.White

        this.inputBox.Width <- this.Width - offset
        this.inputBox.Height <- offset

        this.Controls.Add (this.textBox, 0, 0)
        this.Controls.Add (this.nickList, 1, 0)
        this.Controls.Add (this.inputBox, 0, 1)
        //this.Controls.Add (this.buttonPanel, 1, 1)

    member this.updateNickBox (nicks: string[]) = 
       for s in nicks do
        this.nickList.Text <- this.nickList.Text + s + Environment.NewLine

    interface UpdateTextBox with 
        member this.updateTextBox text = this.textBox.AppendText text
        member this.updateNickBox text = this.nickList.AppendText text

    member this.emptyNickBox = 
        this.nickList.Clear 

        
    member this.initTabControl f = this.tabControl <- f
    member this.initAddTab f = this.addTab <- f
    member this.updateTextBox text = null
    member this.updateInputBox text = null

    new (xSize, ySize) as this = 
        {
            textBox = new TextBox (); 
            nickList = new TextBox (); 
            inputBox = new TextBox (); 
        }
        then 
        this.init xSize ySize

    new () =
        new TextBoxPanel (windowWidth, windowHeight - 100)

    end