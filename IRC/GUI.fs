module GUI
open System.Windows
open System.Windows.Forms
open System.Drawing
open System.Collections.Generic
open TextBoxPanel
open MenuBar
open Tab
open ServerCommunication

let exitHandler obj x = null

let connectHandler obj x = null

[<AllowNullLiteral>]
type GUIManager  = 
    class
    [<DefaultValue>]
    val mutable private form:Form
    [<DefaultValue>]
    val mutable private mainPanel:Panel
    [<DefaultValue>]
    val mutable private tabControl:Tab
    [<DefaultValue>]
    val mutable private textBoxPanel:TextBoxPanel
    [<DefaultValue>]
    val mutable private textBoxPanelMap:Map<string,TextBoxPanel>
    [<DefaultValue>]
    val mutable private tabList: List<TabPage>
    [<DefaultValue>]
    static val mutable private instance:GUIManager
    [<DefaultValue>]
    val mutable private currentConnection: IRCOp

    static member getInstance () = 
        if GUIManager.instance = null then
            GUIManager.instance <- new GUIManager() 
        GUIManager.instance

    static member destroy () = GUIManager.instance <- null


    (*
    member this.addPage name =
        if this.tabList = null then
            this.tabList <- new List<TabPage>()
        let tab = new TabPage () in
        let tableLayout = new TextBoxPanel () in
        this.textBoxPanel <- tableLayout
        tab.Name <- name
        tab.Text <- name
        tab.Controls.Add tableLayout
        this.tabList.Add tab
        this.tabControl.TabPages.Add tab
        tableLayout.initTabControl this.tabControl
        tableLayout.initAddTab this.addPage *)


    member private this.updateConnection x =
        this.currentConnection <- x

 
    member this.init () = 
        this.form <- new Form (Text="F# IRC", Visible= false, TopMost = true)
        this.form.Width <- TextBoxPanel.windowWidth
        this.form.Height <- TextBoxPanel.windowHeight
        this.mainPanel <- new Panel ()
        this.tabControl <- new Tab()
        this.mainPanel.Width <- TextBoxPanel.windowWidth
        this.mainPanel.Height <- TextBoxPanel.windowHeight - 10
        this.form.AutoSize <- true
        this.tabControl.Width <- TextBoxPanel.windowWidth
        this.tabControl.Height <- TextBoxPanel.windowWidth
        this.tabControl.AutoSize <- true
        //this.exitButton.Click.AddHandler
        let bar = new MenuBar (this.updateConnection) in
        this.form.Controls.Add bar
        this.mainPanel.Controls.Add this.tabControl
        this.tabControl.addTab "Hallo"
        this.tabControl.Location <- new Point(0, 30)
        this.form.Controls.Add this.mainPanel
        //this.mainPanel.Controls.Add this.tabControl

    private new () as this = 
        {}
        then
        this.init ()
            
    member this.updateNickBox nameList = this.textBoxPanel.updateNickBox nameList
    member this.updateTextBox text = this.textBoxPanel.updateTextBox text
    member this.updateInputBox text = this.textBoxPanel.updateInputBox text


    member this.run () = 
        Application.Run(this.form)
    end