using Service_Bus_TMS.API.Middlewares;
using Service_Bus_TMS.BLL;

var builder = WebApplication.CreateBuilder(args);

const string serviceBusTMSPolicy = "Service-Bus-TMSPolicy";

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddPolicy(serviceBusTMSPolicy, policyBuilder =>
{
    policyBuilder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Token-Expired");
}));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

BLLModule.Load(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.UseCors(serviceBusTMSPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
