var recive = new Recive.WorkQueues.Recive();
recive.Queue = "Tasks";
recive.ReciveMessage();
Console.ReadLine();