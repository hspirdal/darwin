﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Darwin.Api.Identity;
using Darwin.Api.Status.Position;
using Darwin.Api.Actions;
using Darwin.Api.Actions.Movement;
using StackExchange.Redis;

namespace Darwin.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Darwin API", Version = "v1" });
			});

			RegisterRedis(services);

			var initialPlayers = new List<Player>() { new Player { Id = 1, Name = "Jools" }, new Player { Id = 2, Name = "Jops" } };
			services.AddSingleton<IPlayerRepository>(c => new PlayerRepository(initialPlayers));
			services.AddSingleton<IPositionRepository, PositionRepository>();
			services.AddSingleton<IActionRepository, ActionRepository>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Darwin API v1");
			});

			app.UseMvc();
		}

		private void RegisterRedis(IServiceCollection services)
		{
			//By connecting here we are making sure that our service
			//cannot start until redis is ready. This might slow down startup,
			//but given that there is a delay on resolving the ip address
			//and then creating the connection it seems reasonable to move
			//that cost to startup instead of having the first request pay the
			//penalty.
			services.AddSingleton(sp =>
			{
				var configuration = new ConfigurationOptions { ResolveDns = true };
				configuration.EndPoints.Add(Configuration["RedisHost"]);
				return ConnectionMultiplexer.Connect(configuration);
			});
		}
	}
}
