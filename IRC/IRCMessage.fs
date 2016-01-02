module IRCMessage


type IRCMessage = 
    class 
    val public command:string 
    val public message:string

    new (cmd:string, msg:string) = 
        { 
            command = cmd; 
            message = msg;
        }
    
    override this.ToString () = "Command: " + this.command + "\n" + "Message: " + this.message + "\n"

    end