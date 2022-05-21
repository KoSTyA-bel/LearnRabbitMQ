var send = new Send.Topic.Send();
Console.ReadLine();
send.Routing = "kern.critical";
send.SendMessage("HELLO");
Console.ReadLine();