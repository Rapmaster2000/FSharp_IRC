module Parser
    open System
    open System.Windows
    open System.Windows.Forms
    open System.Drawing

    type IRCError =  
         | IRCError 


    type IRCReply =
        |  REPLY

(*type IRCCommand =
    | JOIN
    | PART
    | QUIT
    | LIST
    | INVALID*)

    

    let splitText (text:string) =
      text.Split ' ' 
   

    let stripNewLine (text:string) = 
        if text.Contains(Environment.NewLine)  then
            let i = text.IndexOf(Environment.NewLine) in
            text.Remove(i)
         else text       

    let (|JOIN|PART|QUIT|LIST|INVALID|) (text:string) =  
        let t = stripNewLine text in
        let a = t.Split ' ' in 
        let b = a.[0] in
        match b with
        | "/join" -> JOIN
        | "/part" -> PART
        | "/quit" -> QUIT
        | "/list" -> LIST
        | _ -> INVALID

    let getPrefix text =
        (splitText text).[0]

    let getCommand text =
        (splitText text).[1]


    let getParams text =
        let ar = (splitText text) in
            if ar.Length > 1 then
                ar.[1 .. ar.Length - 1] // No CLRF, hence - 2
            else null

    let parseResponse (text:string) =
        //let res  = 
        let prefix = getPrefix text in
        let command = getCommand text in
        match text with
        | JOIN ->    MessageBox.Show("join Command")  |> ignore
        | PART ->    MessageBox.Show("part command")  |> ignore
        | LIST ->    MessageBox.Show("list command")  |> ignore
        | QUIT ->    MessageBox.Show("quit command")  |> ignore
        | INVALID -> MessageBox.Show("Nothing found") |> ignore


