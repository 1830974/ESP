﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Models.Interfaces
{
    public interface ILanguageRefresher
    {
        Task RefreshLanguage();
    }
}
