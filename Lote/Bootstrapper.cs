using Lote.Core;
using Lote.Core.Service;
using Lote.NotifyUtil;
using Lote.ViewModels;
using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.Windows;
using System.IO;
using System.Windows.Threading;
using System.Linq;
using XExten.Advance.LinqFramework;
using Lote.Core.Common;

namespace Lote
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        protected override void OnStart()
        {
            //日志
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Logs/Lote.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            //校验版本
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<IOptionService>().To<OptionService>();
            builder.Bind<IWallpaperService>().To<WallpaperService>();
            builder.Bind<IMusicPlayService>().To<MusicPlayService>();

            builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();
        }

        /// <summary>
        /// 初始化系统相关参数配置
        /// </summary>
        protected override void Configure()
        {
            new Lite().InitDataBase();
            base.Configure();
        }

        /// <summary>
        /// 初始化VM
        /// </summary>
        protected override void Launch()
        {
            base.Launch();
        }

        /// <summary>
        /// 加载首页VM
        /// </summary>
        /// <param name="rootViewModel"></param>
        protected override void DisplayRootView(object rootViewModel)
        {
            base.DisplayRootView(rootViewModel);
        }

        /// <summary>
        ///VM加载完毕
        /// </summary>
        protected override void OnLaunch()
        {
            var navigationController = this.Container.Get<NavigationController>();
            navigationController.Delegate = this.RootViewModel;
            base.OnLaunch();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            FileUtily.Instance.DeleteFolder(Path.Combine(Environment.CurrentDirectory, "Lote.exe.WebView2"));
            base.OnExit(e);
        }

        /// <summary>
        /// 全局异常捕获
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception.InnerException ?? e.Exception, "");
            HandyControl.Controls.MessageBox.Error("服务异常，程序将自动关闭。", "错误");
            GC.Collect();
            Application.Current.Shutdown();
        }
    }
}
