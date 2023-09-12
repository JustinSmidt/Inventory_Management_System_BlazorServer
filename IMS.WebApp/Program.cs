using IMS.WebApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using IMS.UseCases.Plugininterfaces;
using IMS.Plugins.InMemory;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.Products.Interfaces;
using IMS.UseCases.Products;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.Activities;
using IMS.UseCases.Reports.Interfaces;
using IMS.UseCases.Reports;
using IMS.Plugins.EFCoreSqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;

namespace IMS.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var constr = builder.Configuration.GetConnectionString("InventoryManagement");

            //Configure authorizations
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Department", "Administration"));
                //options.AddPolicy("Inventory", policy => policy.RequireClaim("Department", "InventoryManagement"));
                //options.AddPolicy("Sales", policy => policy.RequireClaim("Department", "Sales"));    //users department must be sales
                //options.AddPolicy("Purchasers", policy => policy.RequireClaim("Department", "Purchasing"));
                //options.AddPolicy("Productions", policy => policy.RequireClaim("Department", "ProductionManagement"));
                options.AddPolicy("User", policy => policy.RequireClaim("Department", "User"));
            });

            

            //Configure EF Core for Identity
            builder.Services.AddDbContext<AccountDbContext>(options =>
            {
                options.UseSqlServer(constr);
            });

            //Configure Identity
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<AccountDbContext>();


            //we are adding EFCore into application. We DI EFCore related classes into application
            builder.Services.AddDbContextFactory<IMSContext>(options =>
            {
                options.UseSqlServer(constr);
            });


            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();


            if(builder.Environment.IsEnvironment("Testing"))
            {
                StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);  //now we an load the static assets properly

                builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
                builder.Services.AddSingleton<IProductRepository, ProductRepository>();
                builder.Services.AddSingleton<IInventoryTransactionRepository, InventoryTransactionRepository>();
                builder.Services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();

            }
            else
            {
                builder.Services.AddTransient<IInventoryRepository, InventoryEFCoreRepository>();
                builder.Services.AddTransient<IProductRepository, ProductEFCoreRepository>();
                builder.Services.AddTransient<IInventoryTransactionRepository, InventoryTransactionEFCoreRepository>();
                builder.Services.AddTransient<IProductTransactionRepository, ProductTransactionEFCoreRepository>();
            }

            //Dependency Injection (DI) for Inventory       
            builder.Services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();       
            builder.Services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
            builder.Services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
            builder.Services.AddTransient<IViewInventoryByIdUseCase, ViewInventoryByIdUseCase>();

            //Dependency Injection (DI) for Product
            builder.Services.AddTransient<IViewProductsByNameUseCase, ViewProductsByNameUseCase>();
            builder.Services.AddTransient<IAddProductUseCase, AddProductUseCase>();
            builder.Services.AddTransient<IViewProductByIdUseCase, ViewProductByIdUseCase>();
            builder.Services.AddTransient<IEditProductUseCase, EditProductUseCase>();

            //Dependency Injection (DI) for Inventory actvities
            builder.Services.AddTransient<IPurchaseInventoryUseCase , PurchaseInventoryUseCase>();

            //Dependency Injection (DI) for Product activities
            builder.Services.AddTransient<IProduceProductUseCase , ProduceProductUseCase>();
            builder.Services.AddTransient<ISellProductUseCase, SellProductUseCase>();


            //Dependency Injection (DI) for Reporting Inventory Transactions
            builder.Services.AddTransient<ISearchInventoryTransactionUseCase, SearchInventoryTransactionsUseCase>();


            //Dependency Injection (DI) for Reporting Product Transactions
            builder.Services.AddTransient<ISearchProductTransactionUseCase, SearchProductTransactionUseCase>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //////
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}