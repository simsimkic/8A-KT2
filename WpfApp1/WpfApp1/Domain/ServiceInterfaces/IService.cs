﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Observer;
using WpfApp1.Model;

namespace WpfApp1.Domain.ServiceInterfaces
{
    public interface IService<T>
    {
        void Save();
        void Update(T entity);
        List<T> GetAll();
        T Get(int id);
    }
}
