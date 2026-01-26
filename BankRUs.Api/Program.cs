using BankRUs.Application.UseCases.OpenAccount;

var builder = WebApplication.CreateBuilder(args);

//// Samma instans av CustomerService ska vara tillgänglig
//// för samtliga klasser inom ett anrop.
//// Varje request får sin egna instans av CustomerService
//builder.Services.AddScoped<CustomerService>();

// Det finns enbart en instans av CustomerService som delas
// av alla komponenter i applikationen, över applikations livstid.

//// Varje enskild komponent som begär en CustomerService får sin egna
//// instans av denna.
//builder.Services.AddTransient<CustomerService>();

builder.Services.AddControllers();

builder.Services.AddScoped<OpenAccountHandler>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
