var builder = WebApplication.CreateBuilder(args);
//add container
var app = builder.Build();
//configure request pipeline

app.Run();
