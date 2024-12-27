var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
//add container
var app = builder.Build();
//configure request pipeline

app.MapCarter();
app.Run();
