using System;

namespace cassandra_app.cassandra.Models
{
  /// <summary>
  /// 
  /// </summary>
  public class Theater
  {
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Theater(Guid id, string name)
    {
      this.Id = id;
      this.Name = name;
    }
  }
}