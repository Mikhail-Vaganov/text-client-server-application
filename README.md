# Client-server system for text message distribution

The project represents a simple client-server model, which allows one to distribute text messages among client applications connected to the server.

The system satisfies the following requirements:
 1. The number of possible connected to the server clients is limited by the number of free TCP ports on the server host.
 2. The client application has an editable text field and a button, by pushing which one can send the message from the text box to the server.
 3. The other clients receive the message, sent form that client application, and display it in a dedicated text field.
 4.	On the server side the message is written in a file as a single line. The lines in the file are sorted in alphabetical order.
 5. During the work of the server one can open the file from any text processor or editor (e.g. notepad.exe).
 6. The client application has a button, by pushing which all the text from that file can be requested from the server and be displayed in the dedicated text field.
 7.	There are some unit and functional tests. It is more appropriate to use integration test for such systems.
