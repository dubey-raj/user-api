﻿using System;
using System.Collections.Generic;

namespace UserService.DataStorage.DAO;

public partial class Scope
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ClientScope> ClientScopes { get; set; } = new List<ClientScope>();
}
