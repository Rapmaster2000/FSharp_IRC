module Tab

open System
open System.Net
open System.Windows
open System.Windows.Forms
open System.Net.Sockets
open System.Collections.Generic
open System.Drawing

open TextBoxPanel

[<AllowNullLiteral>]
type Tab =
    class 
    inherit TabControl
    val mutable private tabPages:Dictionary<String, TextBoxPanel>

    member public this.closeActiveTab =
        let tab = this.SelectedTab in
        if this.TabCount > 1 then
            if box tab <> null then
                tab.Dispose ()
                this.tabPages.Remove tab.Name |> ignore
                
    member public this.closeTab name =
        let i = 0 in
        if this.TabPages.Count > 1 then
            while i < this.TabPages.Count do
             if this.TabPages.[i].Name = name then
                 this.TabPages.[i].Dispose ()
                 this.tabPages.Remove name |> ignore
        
    member public this.addTab name =
        let panel = new TextBoxPanel () in
        let tab = new TabPage () in
        //this.textBoxPanel <- tableLayout
        tab.Name <- name
        tab.Text <- name
        tab.Controls.Add panel
        this.tabPages.Add (name, panel);
        this.AutoSize <- true
        this.TabPages.Add tab
        //tab.Controls.Add tableLayout
        //this.tabList.Add tab
        //this.TabPages.Add
        //this.initTabControl this.tabControl
        //tableLayout.initAddTab this.addPage

    new () = {tabPages = new Dictionary<String, TextBoxPanel>()}
    end