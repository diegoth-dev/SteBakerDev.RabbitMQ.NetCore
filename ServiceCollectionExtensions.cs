using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SteBakerDev.EventBus;
using SteBakerDev.EventBus.Abstractions;
using SteBakerDev.EventBusRabbitMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace SteBakerDev.RabbitMQ
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRabbitMQPersistentConnection(this IServiceCollection services, string hostName, string userName, string password, int retryCount = 5)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = hostName,
                    UserName = userName,
                    Password = password
                };

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
        }

        public static void AddRabbitMQPersistentConnection(this IServiceCollection services, IConnectionFactory connectionFactory, int retryCount = 5)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                return new DefaultRabbitMQPersistentConnection(connectionFactory, logger, retryCount);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="brokerName">Name of the Broker / Exchange</param>
        /// <param name="subscriptionClientName">Name of the subscribers queue</param>
        /// <param name="retryCount"></param>
        public static void RegisterEventBus(this IServiceCollection services, string brokerName, string subscriptionClientName, int retryCount = 5)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetService<IRabbitMQPersistentConnection>();

                if (rabbitMQPersistentConnection == null)
                {
                    throw new Exception("RabbitMQPersistentConnection has not been setup");
                }
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, brokerName, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
