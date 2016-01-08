module ObserverListener

module Listener = 
    [<AllowNullLiteral>]
    type Listener<'a> = 
     abstract notify: 'a -> unit

module Observer =
    open Listener
    open IRCMessage 
    [<AllowNullLiteral>]
    type Observer = 
     abstract registerListener: Listener<IRCMessage> -> unit
     abstract removeListener: Listener<IRCMessage> -> unit