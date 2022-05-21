List<Task> tasks = new();
var recive1 = new Recive.Topic.Recive();
recive1.Name = "First";
recive1.Topics.Add("#");
tasks.Add(Task.Factory.StartNew(() => recive1.ReciveMessage()));

var recive2 = new Recive.Topic.Recive();
recive2.Name = "Secons";
recive2.Topics.Add("kern.*");
tasks.Add(Task.Factory.StartNew(() => recive2.ReciveMessage()));

var recive3 = new Recive.Topic.Recive();
recive3.Name = "Third";
recive3.Topics.Add("*.critical");
tasks.Add(Task.Factory.StartNew(() => recive3.ReciveMessage()));

var recive4 = new Recive.Topic.Recive();
recive4.Name = "Fourth";
recive4.Topics.Add("kern.*");
recive4.Topics.Add("*.critical");
tasks.Add(Task.Factory.StartNew(() => recive4.ReciveMessage()));

Console.ReadLine();