﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.IE.Qwiq.Mapper.Attributes
{
    public class WorkItemLinksMapperStrategy : IWorkItemMapperStrategy
    {
        private readonly IPropertyInspector _inspector;
        private readonly IWorkItemStore _store;

        public WorkItemLinksMapperStrategy(IPropertyInspector inspector, IWorkItemStore store)
        {
            _inspector = inspector;
            _store = store;
        }

        public void Map(Type targeWorkItemType, IWorkItem sourceWorkItem, object targetWorkItem, IWorkItemMapper workItemMapper)
        {
            var linkProperties = _inspector.GetAnnotatedProperties(targeWorkItemType, typeof(WorkItemLinkAttribute));
            foreach (var property in linkProperties)
            {
                var def = _inspector.GetAttribute<WorkItemLinkAttribute>(property);
                if (def != null)
                {
                    var linkType = def.GetLinkName();

                    var ids = sourceWorkItem.WorkItemLinks
                            .Where(wil => wil.LinkTypeEnd.ImmutableName == linkType)
                            .Select(wil => wil.TargetId).ToArray();

                    if (ids.Any())
                    {

                        var propertyType = def.GetWorkItemType();
                        var workItems = _store.Query(ids);
                        IList results = (IList)typeof(List<>).MakeGenericType(propertyType).GetConstructor(new[] { typeof(int) }).Invoke(new object[] { workItems.Count() });

                        var createdItems = workItemMapper.Create(propertyType, workItems);
                        foreach (var item in createdItems)
                        {
                            results.Add(item);
                        }

                        property.SetValue(targetWorkItem, results);
                    }
                }
            }
        }
    }
}