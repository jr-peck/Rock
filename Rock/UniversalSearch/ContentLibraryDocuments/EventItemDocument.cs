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

using System;
using System.Collections.Generic;
using System.Linq;

using Rock.Model;
using Rock.UniversalSearch.IndexModels.Attributes;
using Rock.Utility;
using Rock.Web.Cache;

namespace Rock.UniversalSearch.ContentLibraryDocuments
{
    /// <summary>
    /// The indexed details of an event item.
    /// </summary>
    /// <remarks>
    /// This index model is not used by Universal Search results. It is only
    /// used by the Content Library system, but it is stored in the Universal
    /// Search index database.
    /// </remarks>
    /// <seealso cref="DocumentBase" />
    [IndexName( "ContentLibrary_EventItemDocument" )]
    [SystemGuid.EntityTypeGuid( "A62982DC-40EC-4E13-83B5-E86B9BDD44F6" )]
    internal class EventItemDocument : DocumentBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of upcoming occurrence dates and times for
        /// this event item document.
        /// </summary>
        /// <value>
        /// The list of upcoming occurrence dates and times.
        /// </value>
        [RockIndexField( Type = IndexFieldType.Date, Index = IndexType.NotIndexed )]
        public List<DateTime> EventItemOccurrences { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new document from the <see cref="EventItem"/>.
        /// </summary>
        /// <param name="eventItem">The item to use as the source for this document.</param>
        /// <param name="source">The library source this document belongs to.</param>
        /// <returns>A new instance of <see cref="EventItemDocument"/> that represents the content channel item.</returns>
        internal static EventItemDocument LoadByModel( EventItem eventItem, ContentLibrarySourceCache source )
        {
            var documentId = GetDocumentId( eventItem.Id, source.Id );
            var document = new EventItemDocument();
            document.SourceIndexModel = "Rock.Model.ContentChannelItem";

            // Try to get the old index so we can fill in trending values.
            var oldIndex = IndexContainer.GetActiveComponent()?.GetDocumentById( typeof( EventItemDocument ), documentId.ToString() );

            document.Id = documentId;
            document.EntityId = eventItem.Id;
            document.Name = eventItem.Name;
            document.Content = eventItem.Summary;
            document.SourceId = source.Id;
            document.SourceGuid = source.Guid;
            document.SourceIdKey = IdHasher.Instance.GetHash( source.Id );
            document.SourceType = typeof( EventCalendar ).FullName;
            document.ItemType = typeof( EventItem ).FullName;
            document.IsTrending = oldIndex?[nameof( IsTrending )].ToString().AsBoolean() ?? false;
            document.TrendingRank = oldIndex?[nameof( TrendingRank )].ToString().AsInteger() ?? -1;
            document.Segments = new List<int>() { 1, 2, 3 };
            // index.RequestFilters = ""; TODO

            var dates = eventItem.EventItemOccurrences
                .Where( o => o.NextStartDateTime.HasValue && o.NextStartDateTime.Value >= RockDateTime.Now )
                .Select( o => o.NextStartDateTime.Value )
                .OrderBy( d => d )
                .ToList();

            if ( dates.Any() )
            {
                document.RelevanceDateTime = dates.First();
                document.EventItemOccurrences = dates.Take( source.OccurrencesToShow ).ToList();
            }
            else
            {
                document.RelevanceDateTime = null;
                document.EventItemOccurrences = new List<DateTime>();
            }

            document.AddIndexableAttributes( eventItem );

            return document;
        }

        #endregion
    }
}
