﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lote.Core.Service.DTO
{
    public class LoteMangaHistoryDTO
    {
        public Guid Id { get; set; }
        public long Span { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string TagKey { get; set; }
        public int Index { get; set; }
        public string Chapters { get; set; }
    }
}
