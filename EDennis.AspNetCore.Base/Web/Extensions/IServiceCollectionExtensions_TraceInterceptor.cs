using Castle.DynamicProxy;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public static class IServiceCollectionExtensions_TraceInterceptor {

        
        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2, TConstructorArgument3,
            TConstructorArgument4, TConstructorArgument5, TConstructorArgument6,
            TConstructorArgument7, TConstructorArgument8, TConstructorArgument9,
            TConstructorArgument10>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();
                var arg3 = f.GetRequiredService<TConstructorArgument3>();
                var arg4 = f.GetRequiredService<TConstructorArgument4>();
                var arg5 = f.GetRequiredService<TConstructorArgument5>();
                var arg6 = f.GetRequiredService<TConstructorArgument6>();
                var arg7 = f.GetRequiredService<TConstructorArgument7>();
                var arg8 = f.GetRequiredService<TConstructorArgument8>();
                var arg9 = f.GetRequiredService<TConstructorArgument9>();
                var arg10 = f.GetRequiredService<TConstructorArgument10>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9,arg10 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }

        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2, TConstructorArgument3,
            TConstructorArgument4, TConstructorArgument5, TConstructorArgument6,
            TConstructorArgument7, TConstructorArgument8, TConstructorArgument9>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();
                var arg3 = f.GetRequiredService<TConstructorArgument3>();
                var arg4 = f.GetRequiredService<TConstructorArgument4>();
                var arg5 = f.GetRequiredService<TConstructorArgument5>();
                var arg6 = f.GetRequiredService<TConstructorArgument6>();
                var arg7 = f.GetRequiredService<TConstructorArgument7>();
                var arg8 = f.GetRequiredService<TConstructorArgument8>();
                var arg9 = f.GetRequiredService<TConstructorArgument9>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }

        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2, TConstructorArgument3,
            TConstructorArgument4, TConstructorArgument5, TConstructorArgument6,
            TConstructorArgument7, TConstructorArgument8>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();
                var arg3 = f.GetRequiredService<TConstructorArgument3>();
                var arg4 = f.GetRequiredService<TConstructorArgument4>();
                var arg5 = f.GetRequiredService<TConstructorArgument5>();
                var arg6 = f.GetRequiredService<TConstructorArgument6>();
                var arg7 = f.GetRequiredService<TConstructorArgument7>();
                var arg8 = f.GetRequiredService<TConstructorArgument8>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }


        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2, TConstructorArgument3,
            TConstructorArgument4, TConstructorArgument5, TConstructorArgument6,
            TConstructorArgument7>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();
                var arg3 = f.GetRequiredService<TConstructorArgument3>();
                var arg4 = f.GetRequiredService<TConstructorArgument4>();
                var arg5 = f.GetRequiredService<TConstructorArgument5>();
                var arg6 = f.GetRequiredService<TConstructorArgument6>();
                var arg7 = f.GetRequiredService<TConstructorArgument7>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }

        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2, TConstructorArgument3,
            TConstructorArgument4, TConstructorArgument5, TConstructorArgument6>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();
                var arg3 = f.GetRequiredService<TConstructorArgument3>();
                var arg4 = f.GetRequiredService<TConstructorArgument4>();
                var arg5 = f.GetRequiredService<TConstructorArgument5>();
                var arg6 = f.GetRequiredService<TConstructorArgument6>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1, arg2, arg3, arg4, arg5, arg6 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }


        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2, TConstructorArgument3,
            TConstructorArgument4, TConstructorArgument5>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();
                var arg3 = f.GetRequiredService<TConstructorArgument3>();
                var arg4 = f.GetRequiredService<TConstructorArgument4>();
                var arg5 = f.GetRequiredService<TConstructorArgument5>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1, arg2, arg3, arg4, arg5 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }


        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2, TConstructorArgument3,
            TConstructorArgument4>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();
                var arg3 = f.GetRequiredService<TConstructorArgument3>();
                var arg4 = f.GetRequiredService<TConstructorArgument4>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1, arg2, arg3, arg4 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }


        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2, TConstructorArgument3>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();
                var arg3 = f.GetRequiredService<TConstructorArgument3>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1, arg2, arg3 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }


        public static IServiceCollection AddScoped<TInterface, TImplementation,
            TConstructorArgument1, TConstructorArgument2>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);

                var arg1 = f.GetRequiredService<TConstructorArgument1>();
                var arg2 = f.GetRequiredService<TConstructorArgument2>();

                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { arg1, arg2 },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }

        /// <summary>
        /// This overload must be for a class that has exactly one injected
        /// constructor argument whose type is <code>ILogger<TInterface></code>
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddScoped<TInterface, TImplementation>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface {

            return services.AddScoped<TInterface>(f => {

                var loggers = f.GetRequiredService<IEnumerable<ILogger<TInterface>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);


                var repo = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation),
                        new object[] { activeLogger },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });
        }




    }
}
