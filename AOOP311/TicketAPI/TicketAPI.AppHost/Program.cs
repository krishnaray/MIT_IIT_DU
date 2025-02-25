var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TicketAPI>("ticketapi");

builder.Build().Run();
