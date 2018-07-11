using AutoMapper;
using BackgroundPublishWithMediator;
using BackgroundPublishWithMediator.Controllers;
using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundPublishWithMediator {
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<BackgroundNotificationHostedService>();
            services.AddSingleton<IBackgroundNotificationQueue, BackgroundNotificationQueue>();
            services.AddSingleton<IBackgroundPublisher, BackgroundPublisher>();
            services.AddAutoMapper();
            services.AddMediatR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();

        }
    }
}
