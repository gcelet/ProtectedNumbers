// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Repositories;

using ProtectedNumbers.Samples.Models;

public class SampleObjectRepository
{
  public SampleObjectRepository()
  {
    Db =
    [
      new SampleObject
      {
        Id = ProtectedNumber.From(3268149285222967951L, null),
        Name = "3268149285222967951"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(4370645650355927116L, null),
        Name = "4370645650355927116"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(2849208538696462814L, null),
        Name = "2849208538696462814"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(2737488684378130375L, null),
        Name = "2737488684378130375"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(3165521510852862138L, null),
        Name = "3165521510852862138"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(3367344455419562669L, null),
        Name = "3367344455419562669"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(4046037197419659517L, null),
        Name = "4046037197419659517"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(4605951185251650790L, null),
        Name = "4605951185251650790"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(2831023411971416963L, null),
        Name = "2831023411971416963"
      },
      new SampleObject
      {
        Id = ProtectedNumber.From(4018378694012108869L, null),
        Name = "4018378694012108869"
      },
    ];
  }

  private List<SampleObject> Db { get; }

  public IEnumerable<SampleObject> GetAll() => Db;

  public SampleObject? GetById(ProtectedNumber protectedNumber)
  {
    SampleObject? sampleObject = Db.FirstOrDefault(i => protectedNumber.Equals(i.Id));

    return sampleObject;
  }

  public IEnumerable<SampleObject> Search(SampleObjectSearch search)
  {
    IQueryable<SampleObject> query = Db.AsQueryable();

    if (search.Id.HasValue)
    {
      query = query.Where(s => s.Id.HasValue && s.Id.Value.Equals(search.Id.Value));
    }

    if (search.Ids is { Length: > 0 })
    {
      query = query.Where(s => s.Id.HasValue && search.Ids.Any(id => id.Equals(s.Id.Value)));
    }

    List<SampleObject> results = query.ToList();

    return results;
  }

  public SampleObject? Save(ProtectedNumber? id, Action<SampleObject> changeAction)
  {
    SampleObject? sampleObject = null;

    if (id.HasValue)
    {
      sampleObject = GetById(id.Value);
    }
    else
    {
      sampleObject = new()
      {
        Id = ProtectedNumber.From(Random.Shared.NextInt64(100000000000000000L, 999999999999999999L), null)
      };
      Db.Add(sampleObject);
    }

    if (sampleObject == null)
    {
      return null;
    }

    changeAction.Invoke(sampleObject);

    return sampleObject;
  }
}
