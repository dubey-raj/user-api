﻿using System;
using System.Collections.Generic;

namespace UserService.DataStorage.DAO;

public partial class ClientScope
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int ScopeId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Scope Scope { get; set; } = null!;
}
