﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Contracts
{
    public interface IHandler<in TEventEntity> where TEventEntity : IDomainEvent
    {
        void Handle(TEventEntity args);

    }
}
