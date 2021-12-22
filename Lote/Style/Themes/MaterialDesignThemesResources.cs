using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lote.Style.Themes
{
    public class MaterialDesignThemesResources: ResourceDictionary
    {
        private static List<ResourceDictionary> _controlsResources;

        public static List<ResourceDictionary> ControlsResources
        {
            get
            {
                if (_controlsResources == null)
                {
                    List<ResourceDictionary> Result = new List<ResourceDictionary>();
                    Result.Add(new ResourceDictionary
                    {
                        Source = ApplicationHelper.GetAbsoluteUri("MaterialDesignThemes.Wpf", "Themes/MaterialDesignTheme.Light.xaml")
                    });

                    Result.Add(new ResourceDictionary
                    {
                        Source = ApplicationHelper.GetAbsoluteUri("MaterialDesignThemes.Wpf", "Themes/MaterialDesignTheme.Defaults.xaml")
                    });

                    Result.Add(new ResourceDictionary
                    {
                        Source = ApplicationHelper.GetAbsoluteUri("MaterialDesignColors", "Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml")
                    });

                    Result.Add(new ResourceDictionary
                    {
                        Source = ApplicationHelper.GetAbsoluteUri("MaterialDesignColors", "Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml")
                    });

                    _controlsResources = Result;
                }

                return _controlsResources;
            }
        }

        //
        // 摘要:
        //     Initializes a new instance of the Theme class.
        public MaterialDesignThemesResources()
        {
            ControlsResources.ForEach(Resources =>
            {
                base.MergedDictionaries.Add(Resources);
            });
          
        }
    }
}
