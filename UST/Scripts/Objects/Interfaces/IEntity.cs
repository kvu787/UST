using System;

namespace UST.Objects.Interfaces;

public interface IEntity {
    Guid Id { get; set; }
    string Name { get; set; }
}
