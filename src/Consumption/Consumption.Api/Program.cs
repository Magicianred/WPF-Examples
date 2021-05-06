/*
*
* 文件名    ：Program     
* 更新时间  : 2020-05-21 11:44 
*/


namespace Consumption.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.EFCore.Context;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Writers;
    using NLog.Web;

    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /*
        *  首次运行项目须知:
        *  1.请检查 appsettings.json 中 MySqlNoteConnection 连接是否与当前的环境一致
        *  2.请确保 数据库的迁移文件已经更新到你的mysql当中。
        *    2.1. 打开程序包管理控制台, 确保API项目为启动项
        *    2.2. 默认项目设定EfCore, 控制台输入 update-database 将当前存在的迁移文件生成到数据库当中
        *   
        */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var host = CreateHostBuilder(args).Build();
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// 创建主机构建器
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                     .ConfigureLogging(logging =>
                     {
                         logging.ClearProviders();
                         logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                     }).UseNLog()
                     .UseUrls("http://*:5001");
                });
    }
}
