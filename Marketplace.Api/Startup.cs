using AutoMapper;
using Marketplace.Domain.Interface.Integrations.caching;
using Marketplace.Domain.Interface.Integrations.Email;
using Marketplace.Domain.Interface.Integrations.Locality;
using Marketplace.Domain.Interface.Integrations.Merchant;
using Marketplace.Domain.Interface.Integrations.Payment;
using Marketplace.Domain.Interface.Integrations.Storage;
using Marketplace.Domain.Interface.Marketplace;
using Marketplace.Domain.Profiles.Markplace;
using Marketplace.Infra.caching;
using Marketplace.Infra.Context;
using Marketplace.Infra.Repository;
using Marketplace.Infra.Repository.Marketplace;
using Marketplace.Integrations.Email;
using Marketplace.Integrations.Locality;
using Marketplace.Integrations.Payment;
using Marketplace.Integrations.Storage;
using Marketplace.Integrations.Storage.Amazon;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clique Terapia API", Version = "v1" });
            });

            // remover do retorno null e variaveis vazias
            services.AddMvc().AddNewtonsoftJson(op =>
            {
                op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                op.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            // comprimir json com https
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<MarketPlaceContext>(opt =>
                     opt.UseNpgsql(
                         _configuration["connectionstrings:database"],
                         x => x.MigrationsAssembly("Marketplace.Infra")));

            // repositorios
            services.AddScoped(typeof(BaseRepository<>), typeof(BaseRepository<>));

            // integrations individual
            services.AddTransient<ILocality, LocalityIntegrations>();
            services.AddTransient<IEmail, EmailIntegrations>();
            services.AddTransient<IStorage, StorageIntegrations>();
            services.AddTransient<IPayment, PaymentIntegrations>();
            services.AddTransient<IMerchant, MerchantIntegrations>();

            // integrations individual Storage
            services.AddScoped<ICustomCache, CacheApp>();
            services.AddScoped<AmazonStorageClient>();

            // repositorio individual
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupPermissionRepository, GroupPermissionRepository>();
            services.AddScoped<IBankRepository, BankRepository>();

            // services
            services.AddScoped<CategoryService>();
            services.AddScoped<LocationService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<ProviderService>();
            services.AddScoped<LanguageService>();
            services.AddScoped<EmailService>();
            services.AddScoped<TopicService>();
            services.AddScoped<AuthService>();
            services.AddScoped<GroupPermissionService>();
            services.AddScoped<UserService>();
            services.AddScoped<BankService>();
            services.AddScoped<MerchantService>();
            services.AddScoped<MarketplaceService>();

            // validator
            services.AddSingleton<Services.Validators.CustomerValidator>();
            services.AddSingleton<Services.Validators.CustomerAuthValidator>();
            services.AddSingleton<Services.Validators.ProviderValidator>();

            #region ..: AUTO MAPPER :..

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingBase());
                mc.AddProfile(new MappingCustomers());
                mc.AddProfile(new MappingProvider());
                mc.AddProfile(new MappingGroupPermission());
                mc.AddProfile(new MappingUsers());
            });
            services.AddSingleton(mappingConfig.CreateMapper());
            #endregion

            // authorize
            var key = Encoding.ASCII.GetBytes(_configuration["secrets:signingkey"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            //.AddJwtBearer(op => {
            //    op.Authority = "https://securetoken.google.com/marketplace-staging-2f07d";
            //    op.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = "https://securetoken.google.com/marketplace-staging-2f07d",
            //        ValidateAudience = true,
            //        ValidAudience = "marketplace-staging-2f07d",
            //        ValidateLifetime = true
            //    };
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new string[] { "pt-BR" };
            app.UseRequestLocalization(options => options
               .AddSupportedCultures(supportedCultures)
               .AddSupportedUICultures(supportedCultures)
               .SetDefaultCulture("pt-BR")
               .RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
               {
                   return Task.FromResult(new ProviderCultureResult("pt-BR"));
               })));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Services API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseResponseCompression();
        }
    }
}
