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
using Darwin.Api.Identities;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using GameLib.Users;

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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressConsumesConstraintForFormFileParameters = true;
				options.SuppressInferBindingSourcesForParameters = true;
				options.SuppressModelStateInvalidFilter = false;
			});

			services.AddCors(options => options.AddPolicy("CorsPolicy",
			builder =>
			{
				builder.AllowAnyMethod().AllowAnyHeader()
										 .WithOrigins("http://localhost:5001")
										 .WithOrigins("http://localhost:8080")
										 .AllowCredentials();
			}));

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Darwin API", Version = "v1" });
			});

			RegisterRedis(services);

			services.AddSingleton<IIdentityRepository, IdentityRepository>();
			services.AddSingleton<IUserRepository, UserRepository>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IIdentityRepository identityRepository)
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

			app.UseCors("CorsPolicy");

			app.UseMvc();

			// Temp placement until a better home appears..
			identityRepository.AddOrUpdateAsync(new Identity { Id = "1", UserName = "arch", Password = "1234" }).Wait();
			identityRepository.AddOrUpdateAsync(new Identity { Id = "2", UserName = "clip", Password = "1234" }).Wait();
		}

		private void RegisterRedis(IServiceCollection services)
		{
			//By connecting here we are making sure that our service
			//cannot start until redis is ready. This might slow down startup,
			//but given that there is a delay on resolving the ip address
			//and then creating the connection it seems reasonable to move
			//that cost to startup instead of having the first request pay the
			//penalty.
			services.AddSingleton<IConnectionMultiplexer>(sp =>
			{
				var configuration = new ConfigurationOptions { ResolveDns = true };
				configuration.EndPoints.Add(Configuration["RedisHost"]);
				var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);
				connectionMultiplexer.PreserveAsyncOrder = false;
				return connectionMultiplexer;
			});
		}
	}
}
