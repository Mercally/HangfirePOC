var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.HangfireOrchestratorComponent>("hangfireorchestratorcomponent");

builder.AddProject<Projects.WebApiOne>("webapione");

builder.Build().Run();
