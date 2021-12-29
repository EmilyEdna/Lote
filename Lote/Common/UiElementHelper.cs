using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Lote.Common
{
    public class UiElementHelper
    {
        public static List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T> { };
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        TList.Add((T)child);
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }

        public static Task RunNewWindowAsync<TWindow>() where TWindow : Window, new()
        {
            TaskCompletionSource<object> tc = new TaskCompletionSource<object>();
            Thread thread = new Thread(() =>
            {
                TWindow win = new TWindow();
                win.Closed += (d, k) =>
                {
                    Dispatcher.ExitAllFrames();
                };
                win.Show();
                Dispatcher.Run();
                tc.SetResult(null);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tc.Task;
        }
    }
}
