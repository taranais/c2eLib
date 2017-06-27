
# Inject CAOS script from external application under Linux

Explains the protocol used for talking to the game engine while it is running. This lets you inject new code to change or monitor agents and the world.

The Linux version of Docking Station and Creatures 3 listens on a TCP/IP port for CAOS commands. You can find the port used by the last launched instance of the engine by looking at the file ~/.creaturesengine/port. The port defaults to 20001, although if that port is already in use a higher number will be used until a free one is found.

For finer granularity, you can look in ~/.dockingstation/port or ~/.creatures3/port. Of course then your program will only work with that game.

Once connected to localhost on the given port send CAOS commands as text. They can be separated by a line break or a space. When you've finished send the text "rscr" on a line by itself. The server will then send its reply as text and close the connection.

Shell scripts: The easiest way to talk to a TCP/IP socket is to install netcat. This should be available in a package for your distribution, or you can install it from source. Once installed it is called "nc".

Example: This counts how many agents are in the world.

    $ nc localhost `cat ~/.creaturesengine/port`
    outv totl 0 0 0
    outs "\n"
    rscr
    305

Other languages: Most languages have some way of connecting to a TCP/IP socket. If you don't know how to, have a look in the documentation for your language, or ask about.

Timing: You don't have to pause the game to inject commands. The engine accepts all incoming socket connections, and holds on to them until the end of the next tick. Then it runs through and processes all the waiting ones. So if you want to inject 10 scripts per tick (say for a CAOS debugger) then you should open all ten sockets at once and send all the commands at once, then wait for all the replies to come back.

Settings: You can edit the file user.cfg (in either ~/.creatures3 or ~/.dockingstation) to explicitly change the port, or disable security. Add an entry Port with the port number to try first. Add PortSecurity 0 to allow connections from machines other than localhost. Note that this will let any machine on the internet inject CAOS unless you have a firewall. If you have PortSecurity 1 (the default) the Creatures Engine will print on the terminal when someone tries to make a connection from another machine.