using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Project;
using Project.ProviderShift;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMarten(opts =>
{
    opts.Connection(ServicesFixture.ConnectionString);
    opts.AutoCreateSchemaObjects = AutoCreate.All;

    opts.Projections.SelfAggregate<ProviderShiftLive>(ProjectionLifecycle.Live);
    opts.Projections.SelfAggregate<ProviderShiftInline>(ProjectionLifecycle.Inline);
    opts.Projections.SelfAggregate<ProviderShiftAsync>(ProjectionLifecycle.Async);
}).AddAsyncDaemon(DaemonMode.HotCold);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();