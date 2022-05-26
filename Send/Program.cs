var client = new Send.RPC.RpcClient();
client.RoutingKey = "rpc_queue";
var res = client.Call("25");
Console.WriteLine(res);
client.Close();
Console.ReadLine();