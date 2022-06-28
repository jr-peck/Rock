// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

using System.Collections.Generic;

using Rock.Model;
using Rock.UniversalSearch.IndexModels.Attributes;
using Rock.Utility;
using Rock.Web.Cache;

namespace Rock.UniversalSearch.ContentLibraryDocuments
{
    /// <summary>
    /// The indexed details of a content channel item.
    /// </summary>
    /// <remarks>
    /// This index model is not used by Universal Search results. It is only
    /// used by the Content Library system, but it is stored in the Universal
    /// Search index database.
    /// </remarks>
    /// <seealso cref="DocumentBase" />
    [IndexName( "ContentLibrary_ContentChannelItemDocument" )]
    internal class ContentChannelItemDocument : DocumentBase
    {
        #region Methods

        /// <summary>
        /// Creates a new document from the <see cref="ContentChannelItem"/>.
        /// </summary>
        /// <param name="contentChannelItem">The item to use as the source for this document.</param>
        /// <param name="source">The library source this document belongs to.</param>
        /// <returns>A new instance of <see cref="ContentChannelItemDocument"/> that represents the content channel item.</returns>
        internal static ContentChannelItemDocument LoadByModel( ContentChannelItem contentChannelItem, ContentLibrarySourceCache source )
        {
            var documentId = GetDocumentId( contentChannelItem.Id, source.Id );
            var document = new ContentChannelItemDocument();
            document.SourceIndexModel = "Rock.Model.ContentChannelItem";

            // Try to get the old index so we can fill in trending values.
            var oldIndex = IndexContainer.GetActiveComponent()?.GetDocumentById( typeof( ContentChannelItemDocument ), documentId.ToString() );

            document.Id = documentId;
            document.EntityId = contentChannelItem.Id;
            document.Name = contentChannelItem.Title;
            document.Content = contentChannelItem.Content;
            document.SourceId = source.Id;
            document.SourceGuid = source.Guid;
            document.SourceIdKey = IdHasher.Instance.GetHash( source.Id );
            document.SourceType = typeof( ContentChannel ).FullName;
            document.ItemType = typeof( ContentChannelItem ).FullName;
            document.IsTrending = oldIndex?[nameof( IsTrending )].ToString().AsBoolean() ?? false;
            document.TrendingRank = oldIndex?[nameof( TrendingRank )].ToString().AsInteger() ?? -1;
            document.Segments = new List<int>() { 1, 2, 3 };
            // index.RequestFilters = ""; TODO
            document.RelevanceDateTime = contentChannelItem.StartDateTime;

            document.AddIndexableAttributes( contentChannelItem );

            return document;
        }

        #endregion
    }
}
