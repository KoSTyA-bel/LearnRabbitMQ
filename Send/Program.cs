var send = new Send.Exchange.Send();
for (int i = 1; i <= 6; i++)
{
    send.SendMessage(String.Concat(Enumerable.Repeat(".", i))
);
}
