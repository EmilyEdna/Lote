﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lote.Core.Service.DTO
{
    public class LoteNovelHistoryDTO
    {
        public Guid Id { get; set; }

        public long Span { get; set; }
        /// <summary>
        /// 上一页
        /// </summary>

        public string PreviousPage { get; set; }
        /// <summary>
        /// 下一页
        /// </summary>
        public string NextPage { get; set; }
        /// <summary>
        /// 上一章
        /// </summary>
        public string PreviousChapter { get; set; }
        /// <summary>
        ///下一章
        /// </summary>
        public string NextChapter { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 章节
        /// </summary>
        public string ChapterName { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string BookName { get; set; }
    }
}
