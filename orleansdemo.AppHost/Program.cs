var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var clusteringTable = storage.AddTables("clustering");
var grainStorage = storage.AddBlobs("grain-state");
var orleans = builder.AddOrleans("default")
                .WithClustering(clusteringTable)
                .WithGrainStorage("Default", grainStorage);

builder.AddProject<Projects.orleansdemo_server>("silo")
       .WithReference(orleans)
       .WithReplicas(3);

builder.AddProject<Projects.orleansdemo_client>("client")
       .WithReference(orleans.AsClient())
       .WithExternalHttpEndpoints()
       .WithReplicas(3);

builder.Build().Run();
