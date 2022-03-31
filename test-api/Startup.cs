using System;
using System.Net;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using test_api.Filters;
using test_api.Models.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using test_api.Repositories;
using Microsoft.Extensions.Options;
using test_api.Services;

namespace test_api
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

            services.AddOptions();

            services.Configure<JWTOptions>(Configuration.GetSection("Jwt"));
            services.Configure<MongoDBOptions>(Configuration.GetSection("Mongodb"));

            var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

            services.AddCors();
            services.AddHttpClient();

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.Filters.Add(typeof(InvalidOperationExceptionFilter));
            })
            .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

            AddJWTProvider(services);

            AddSwaggerProvider(services);

            AddAutoMapper(services);

            AddTestAPIProvider(services);
        }

        private void AddJWTProvider(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                var jwtOptions = Configuration.GetSection("Jwt").Get<JWTOptions>();
                var key = Convert.FromBase64String(jwtOptions.Key);

                x.IncludeErrorDetails = true;

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    RequireSignedTokens = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                };
            });
        }

        private void AddSwaggerProvider(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TEST API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Scheme = "Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        private void AddAutoMapper(IServiceCollection services)
        {

            services.AddSingleton<IMapper>(provider =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<TestAPIProfile>();
                });

                return new Mapper(config);
            });
        }

        private void AddTestAPIProvider(IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var mongodbOptions = provider.GetService<IOptions<MongoDBOptions>>();

                return new DBContext(mongodbOptions);
            });

            services.AddScoped<WeatherService>();
            services.AddScoped<WeatherRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            const string defaultSwaggerPath = "/swagger/v1/swagger.json";

            ServicePointManager.DefaultConnectionLimit = 600;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase("/test-api");
            app.UseRouting();

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(defaultSwaggerPath, "TEST API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
