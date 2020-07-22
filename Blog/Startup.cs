using AutoMapper;
using Blog.Contracts.CommandInterfeces;
using Blog.Contracts.Queryinterfaces;
using Blog.Contracts.Serviceinterfaces;
using Blog.Data;
using Blog.Extensions;
using Blog.Features.Commands;
using Blog.Features.Queries;
using Blog.Models;
using Blog.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog
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
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration["REDIS_URL"] ?? "127.0.0.1";
                option.InstanceName = "master";
            });

            string postgresUrl = Configuration["DATABASE_URL"];
            if (!String.IsNullOrEmpty(postgresUrl))
            {
                var builder = new PostgreSqlConnectionStringBuilder(postgresUrl)
                {
                    Pooling = true,
                    TrustServerCertificate = true,
                    SslMode = SslMode.Require
                };

                services.AddEntityFrameworkNpgsql()
                        .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.ConnectionString));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 5;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = true; // требуются ли цифры
                opts.User.AllowedUserNameCharacters = null;
                opts.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IDbConnectionFactory>(x => new DbConnectionFactory(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(Startup));

            services.AddElasticsearch();
            services.AddRabbitMQ();

            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IVoteSevice, VoteSevice>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<EmailService>();

            //register queryes
            var queryHandlers = typeof(Startup).Assembly.GetTypes()
             .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));
            foreach (var handler in queryHandlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)), handler);
            }

            //register commands
            var commandHandlers = typeof(Startup).Assembly.GetTypes()
             .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));
            foreach (var handler in commandHandlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)), handler);
            }


            services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                })
                .AddVkontakte(options =>
                {
                    options.ClientId = Configuration["VkClientId"] ?? Passwords.VkClientId;
                    options.ClientSecret = Configuration["VkClientSecret"] ?? Passwords.VkClientSecret;
                    options.Scope.Add("email");
                    options.Events.OnCreatingTicket = (context) =>
                    {
                        JObject json = JObject.Parse(context.User.ToString());
                        context.Identity.AddClaim(new Claim(ClaimTypes.Name, json["first_name"] + " " + json["last_name"]));
                        context.Identity.AddClaim(new Claim("image", json["photo"].ToString()));

                        return Task.CompletedTask;
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["GoogleClientId"] ?? Passwords.GoogleClientId;
                    options.ClientSecret = Configuration["GoogleClientSecret"] ?? Passwords.GoogleClientSecret;
                    options.Scope.Add("profile");
                    options.Events.OnCreatingTicket = (context) =>
                    {
                        JObject json = JObject.Parse(context.User.ToString());
                        context.Identity.AddClaim(new Claim("image", json["picture"].ToString()));

                        return Task.CompletedTask;
                    };
                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            //Nginx
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserRoles(serviceProvider).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var roleCheck = await RoleManager.RoleExistsAsync("God");
            if (roleCheck == false)
            {
                await RoleManager.CreateAsync(new IdentityRole("God"));
            }

            User user = await UserManager.FindByEmailAsync("AZAZF93@GMAIL.COM");
            
            if (user.PasswordHash == null & user.UserName == "Барон-Фон Копатыч")
                await UserManager.AddToRoleAsync(user, "God");
        }
    }
}
