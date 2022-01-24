using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lote.ViewModels.DTO
{
    public class RootViewDTO
    {
        public string FuncName { get; set; }
        public string ImageRoute { get; set; }
        public string CommandParam { get; set; }

        public ObservableCollection<RootViewDTO> Datas()
        {
            var data = new ObservableCollection<RootViewDTO>();
            data.Add(new RootViewDTO
            {
                FuncName = "小说(Ctrl+N)",
                ImageRoute = "/Resource/Assets/btn1.jpg",
                CommandParam = "1"
            });
            data.Add(new RootViewDTO
            {
                FuncName = "轻小说(Ctrl+L)",
                ImageRoute = "/Resource/Assets/btn2.jpg",
                CommandParam = "2"
            });
            data.Add(new RootViewDTO
            {
                FuncName = "动漫(Ctrl+A)",
                ImageRoute = "/Resource/Assets/btn3.jpg",
                CommandParam = "3"
            });
            data.Add(new RootViewDTO
            {
                FuncName = "漫画(Ctrl+M)",
                ImageRoute = "/Resource/Assets/btn4.jpg",
                CommandParam = "4"
            });
            data.Add(new RootViewDTO
            {
                FuncName = "壁纸(Ctrl+W)",
                ImageRoute = "/Resource/Assets/btn5.jpg",
                CommandParam = "5"
            });
            data.Add(new RootViewDTO
            {
                FuncName = "音乐(Ctrl+Y)",
                ImageRoute = "/Resource/Assets/btn6.jpg",
                CommandParam = "6"
            });
            return data;
        }
    }
}
