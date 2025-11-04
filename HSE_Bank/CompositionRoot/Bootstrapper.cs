using System;
using System.Linq; // ВАЖНО: нужно для Single/Remove
using HSE_Bank.Application.Analytics;
using HSE_Bank.Application.Commands.Abstractions;
using HSE_Bank.Application.Commands.Account;
using HSE_Bank.Application.Commands.Bus;
using HSE_Bank.Application.Commands.Bus.Decorators;
using HSE_Bank.Application.Commands.Operation;
using HSE_Bank.Application.Facades;
using HSE_Bank.Application.Seeds;
using HSE_Bank.Domain.Events;
using HSE_Bank.Domain.Factories;
using HSE_Bank.Domain.Policies;
using HSE_Bank.InMemory.Ids;
using HSE_Bank.InMemory.Repositories;
using HSE_Bank.InMemory.State;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.CompositionRoot;

public static class Bootstrapper
    {
        public static ServiceProvider Build()
        {
            var services = new ServiceCollection();

            // Состояние
            services.AddSingleton<AppState>();
            services.AddSingleton<StateAccess>();

            // Генератор Id
            services.AddSingleton<IIdGenerator, GuidIdGenerator>();

            // Доменные сервисы/фабрики/валидации
            services.AddSingleton<IFinanceFactory, ValidatingFinanceFactory>();
            services.AddSingleton<IValidationRule<OperationDraft>, OperationAmountRule>();
            services.AddSingleton<IValidationRule<OperationDraft>, AccountOverdraftRule>();

            // Событийная шина
            services.AddSingleton<IDomainEventBus, InMemoryDomainEventBus>();

            // In-memory stores
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<IOperationStore, OperationStore>();
            services.AddSingleton<ICategoryStore, CategoryStore>();

            // Командная шина
            services.AddSingleton<ICommandBus, CommandBus>();

            // Обработчики команд (реальные)
            services.AddTransient<ICommandHandler<CreateAccount>, CreateAccountHandler>();
            services.AddTransient<ICommandHandler<RenameAccount>, RenameAccountHandler>();
            services.AddTransient<ICommandHandler<AddOperation>,   AddOperationHandler>();
            services.AddTransient<ICommandHandler<DeleteOperation>, DeleteOperationHandler>();

            // Декораторы (оборачиваем каждый конкретный обработчик)
            services.Decorate<ICommandHandler<CreateAccount>>(h =>
                new TimedHandlerDecorator<CreateAccount>(
                    new LoggingHandlerDecorator<CreateAccount>(h, s => Console.WriteLine($"[LOG] {s}")),
                    (n,ms) => Console.WriteLine($"[METRIC] {n}: {ms:F2} ms")));

            services.Decorate<ICommandHandler<RenameAccount>>(h =>
                new TimedHandlerDecorator<RenameAccount>(
                    new LoggingHandlerDecorator<RenameAccount>(h, s => Console.WriteLine($"[LOG] {s}")),
                    (n,ms) => Console.WriteLine($"[METRIC] {n}: {ms:F2} ms")));

            services.Decorate<ICommandHandler<AddOperation>>(h =>
                new TimedHandlerDecorator<AddOperation>(
                    new LoggingHandlerDecorator<AddOperation>(h, s => Console.WriteLine($"[LOG] {s}")),
                    (n,ms) => Console.WriteLine($"[METRIC] {n}: {ms:F2} ms")));

            services.Decorate<ICommandHandler<DeleteOperation>>(h =>
                new TimedHandlerDecorator<DeleteOperation>(
                    new LoggingHandlerDecorator<DeleteOperation>(h, s => Console.WriteLine($"[LOG] {s}")),
                    (n,ms) => Console.WriteLine($"[METRIC] {n}: {ms:F2} ms")));

            // Аналитика (стратегии)
            services.AddSingleton<SumByPeriodStrategy>();
            services.AddSingleton<SumByCategoryStrategy>();
            services.AddSingleton<AnomalyCheckStrategy>();

            // Фасады
            services.AddSingleton<AccountsFacade>();
            services.AddSingleton<OperationsFacade>();
            services.AddSingleton<CategoriesFacade>();
            services.AddSingleton<AnalyticsFacade>();

            // Seeds
            services.AddSingleton<DemoDataBuilder>();

            return services.BuildServiceProvider();
        }

        // Простой helper для декораторов без сторонних DI-контейнеров
        private static IServiceCollection Decorate<T>(this IServiceCollection services, Func<T, T> decorator) where T : class
        {
            // Берём уже зарегистрированный дескриптор
            var descriptor = services.Single(s => s.ServiceType == typeof(T));
            services.Remove(descriptor);

            // Воссоздаём исходную зависимость
            T Factory(IServiceProvider sp)
            {
                object inner =
                    descriptor.ImplementationInstance
                    ?? (descriptor.ImplementationFactory != null
                        ? descriptor.ImplementationFactory(sp)
                        : ActivatorUtilities.CreateInstance(sp, descriptor.ImplementationType!));

                return decorator((T)inner);
            }

            services.Add(new ServiceDescriptor(typeof(T), sp => Factory(sp), descriptor.Lifetime));
            return services;
        }
    }