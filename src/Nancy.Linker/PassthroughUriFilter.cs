﻿namespace Nancy.Linker
{
  using Nancy.Helpers;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// <see cref="IUriFilter"/> that appends all given query string parameters by name.
  /// </summary>
  public class PassthroughUriFilter : UriFilter
  {
    IList<string> queryNamesToPassThrough;

    public PassthroughUriFilter(IEnumerable<string> queryNamesToPassThrough, IUriFilter nextFilter = null) :
      base(nextFilter)
    {
      if (queryNamesToPassThrough == null) throw new ArgumentNullException(nameof(queryNamesToPassThrough));

      this.queryNamesToPassThrough = queryNamesToPassThrough.ToList();
    }

    protected override Uri OnApply(Uri uri, NancyContext context)
    {
      if (uri == null)
        throw new ArgumentNullException(nameof(uri));
      if (context == null)
        throw new ArgumentNullException(nameof(context));

      var query = HttpUtility.ParseQueryString(uri.Query);

      foreach (string name in this.queryNamesToPassThrough)
      {
        var queryValue = context.Request.Query[name];
        if (!queryValue.HasValue) continue;

        query.Add(name, queryValue.Value.ToString());
      }

      var builder = new UriBuilder(uri);
      builder.Query = query.ToString();

      return builder.Uri;
    }
  }
}
