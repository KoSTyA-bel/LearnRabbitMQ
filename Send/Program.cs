var send = new Send.HelloWorld.Send();
send.Queue = "Tasks";
for (int i = 1; i <= 6; i++)
{
    send.SendMessage(String.Concat(Enumerable.Repeat(".", i))
);
}
