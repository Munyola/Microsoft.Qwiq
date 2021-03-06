using Microsoft.Qwiq.Exceptions;

using Tfs = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Microsoft.Qwiq.Client.Soap
{
    internal class RelatedLink : Qwiq.RelatedLink
    {
        internal RelatedLink(Tfs.RelatedLink relatedLink)
            : base(
                   relatedLink.RelatedWorkItemId,
                   ExceptionHandlingDynamicProxyFactory.Create<IWorkItemLinkTypeEnd>(new WorkItemLinkTypeEnd(relatedLink.LinkTypeEnd)),
                   relatedLink.Comment)
        {
        }
    }
}