var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin()
                      .AddDatabase("DefaultConnection");

builder.AddProject<Projects.BBV_HR_Api>("api")
       .WithReference(postgres)
       .WaitFor(postgres);

builder.Build().Run();
