module IRCMessage


type IRCMessage = 
    class 
    val command:string 
    val message:string

    new (cmd, msg) = 
        { 
            command = cmd; 
            message = msg;
        }
    
    override this.ToString () = "Command: " + this.command + "\n" + "Message: " + this.message + "\n"

    end