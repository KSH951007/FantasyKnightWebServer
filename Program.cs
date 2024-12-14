
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace FantasyKnightWebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer(); // API Ž���⸦ ���
            builder.Services.AddSwaggerGen();           // Swagger Generator�� ���
            // Add services to the container.
            builder.Services.AddDbContext<AppDBContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"),
        new MySqlServerVersion(new Version(11, 4, 3)) // MariaDB ������ �°� ����
    ));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


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
        }
    }
}
