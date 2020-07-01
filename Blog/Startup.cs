using AutoMapper;
using Blog.Contracts.CommandInterfeces;
using Blog.Contracts.Queryinterfaces;
using Blog.Contracts.Serviceinterfaces;
using Blog.Data;
using Blog.Features.Commands;
using Blog.Features.Queries;
using Blog.Models;
using Blog.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Claims;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 5;   // ����������� �����
                opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
                opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
                opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
                opts.Password.RequireDigit = true; // ��������� �� �����
                opts.User.AllowedUserNameCharacters = null;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IDbConnectionFactory>(x => new DbConnectionFactory(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(Startup));
            
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IVoteSevice, VoteSevice>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<EmailService>();

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
                .AddCookie()
                .AddVkontakte(options =>
                {
                    options.ClientId = Passwords.VkClientId;
                    options.ClientSecret = Passwords.VkClientSecret;
                    options.Events.OnCreatingTicket = (context) =>
                    {
                        JObject json = JObject.Parse(context.User.ToString());
                        context.Identity.AddClaim(new Claim(ClaimTypes.Name, json["first_name"] + " " + json["last_name"]));
                        context.Identity.AddClaim(new Claim("image", json["photo"].ToString()));

                        return System.Threading.Tasks.Task.CompletedTask;
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Passwords.ClientId;
                    options.ClientSecret = Passwords.ClientSecret;
                    options.Scope.Add("profile");
                    options.Events.OnCreatingTicket = (context) =>
                    {
                        JObject json = JObject.Parse(context.User.ToString());
                        context.Identity.AddClaim(new Claim("image", json["picture"].ToString()));

                        return System.Threading.Tasks.Task.CompletedTask;
                    };
                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}
