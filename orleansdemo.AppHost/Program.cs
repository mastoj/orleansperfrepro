using System.Diagnostics;
using Microsoft.Extensions.Configuration;

EnsureDeveloperControlPaneIsNotRunning();

var builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
       ["AppHost:BrowserToken"] = "",
});

var storage = builder.AddAzureStorage("storage").RunAsEmulator(e => e.WithImageTag("3.34.0"));
var clusteringTable = storage.AddTables("clustering");
var grainStorage = storage.AddBlobs("grain-state");
var orleans = builder.AddOrleans("default")
                .WithClustering(clusteringTable)
                .WithGrainStorage("Default", grainStorage);

builder.AddProject<Projects.orleansdemo_server>("silo")
       .WithReference(orleans)
       .WaitFor(storage)
       .WithReplicas(3);

builder.AddProject<Projects.orleansdemo_client>("client")
       .WithReference(orleans.AsClient())
       .WithExternalHttpEndpoints()
       .WaitFor(storage)
       .WithReplicas(3);

builder.Build().Run();


void EnsureDeveloperControlPaneIsNotRunning()
{
       const string processName = "dcpctrl"; // The Aspire Developer Control Pane process name

       var process = Process.GetProcesses()
           .SingleOrDefault(p => p.ProcessName.Contains(processName, StringComparison.OrdinalIgnoreCase));

       if (process == null) return;

       Console.WriteLine($"Shutting down developer control pane from previous run. Process: {process.ProcessName} (ID: {process.Id})");

       Thread.Sleep(TimeSpan.FromSeconds(5)); // Allow Docker containers to shut down to avoid orphaned containers

       try
       {
              process.Kill();
              Console.WriteLine($"Process {process.Id} killed successfully.");
       }
       catch (Exception ex)
       {
              Console.WriteLine($"Failed to kill process {process.Id}: {ex.Message}");
       }
}