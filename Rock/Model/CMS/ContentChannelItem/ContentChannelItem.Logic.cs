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
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

using Rock.Data;
using Rock.Lava;
using Rock.UniversalSearch;
using Rock.UniversalSearch.ContentLibraryDocuments;
using Rock.Web.Cache;

namespace Rock.Model
{
    public partial class ContentChannelItem
    {
        /// <summary>
        /// Gets the parent authority.
        /// </summary>
        /// <value>
        /// The parent authority.
        /// </value>
        [NotMapped]
        public override Security.ISecured ParentAuthority
        {
            get
            {
                return ContentChannel != null ? ContentChannel : base.ParentAuthority;
            }
        }

        /// <summary>
        /// Gets the primary slug.
        /// </summary>
        /// <value>
        /// The primary alias.
        /// </value>
        [NotMapped]
        [LavaVisible]
        public virtual string PrimarySlug
        {
            get
            {
                return ContentChannelItemSlugs.Select( a => a.Slug ).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a value indicating whether [allows interactive bulk indexing].
        /// </summary>
        /// <value>
        /// <c>true</c> if [allows interactive bulk indexing]; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        [NotMapped]
        public bool AllowsInteractiveBulkIndexing
        {
            get
            {
                return true;
            }
        }

        #region IRockContentLibraryIndexable Methods

        /// <inheritdoc/>
        void IRockContentLibraryIndexable.IndexContentLibraryDocument( int id )
        {
            using ( var rockContext = new RockContext() )
            {
                var itemEntity = new ContentChannelItemService( rockContext ).Get( id );

                if ( itemEntity == null )
                {
                    return;
                }

                // Create or update any indexed documents for content library sources.
                var contentChannelEntityTypeId = EntityTypeCache.Get<ContentChannel>().Id;
                var sources = ContentLibrarySourceCache.All()
                    .Where( s => s.EntityTypeId == contentChannelEntityTypeId
                        && s.EntityId == itemEntity.ContentChannelId )
                    .ToList();

                foreach ( var source in sources )
                {
                    var indexItem = ContentChannelItemDocument.LoadByModel( itemEntity, source );
                    IndexContainer.IndexDocument( indexItem );
                }
            }
        }

        /// <inheritdoc/>
        void IRockContentLibraryIndexable.DeleteContentLibraryDocument( int id )
        {
            // Delete all content channel item documents with this entity id.
            IndexContainer.DeleteDocumentByProperty( typeof( ContentChannelItemDocument ),
                nameof( ContentChannelItemDocument.EntityId ),
                id );
        }

        /// <inheritdoc/>
        void IRockContentLibraryIndexable.IndexAllContentLibraryDocuments( int? sourceId )
        {
            using ( var rockContext = new RockContext() )
            {
                // Get all the content channel identifiers that need have items
                // in need of indexing.
                var contentChannelEntityTypeId = EntityTypeCache.Get<ContentChannel>().Id;
                var sources = new ContentLibrarySourceService( rockContext ).Queryable()
                    .Where( s => s.EntityTypeId == contentChannelEntityTypeId
                        && ( !sourceId.HasValue || s.Id == sourceId.Value ) );

                // Create a pure-SQL join to select all content channel items that
                // belong to one of the sources.
                var items = new ContentChannelItemService( rockContext ).Queryable()
                    .AsNoTracking()
                    .Join( sources, cci => cci.ContentChannelId, s => s.EntityId, ( cci, s ) => new
                    {
                        Item = cci,
                        SourceId = s.Id
                    } )
                    .ToList();

                foreach ( var pair in items )
                {
                    // Make sure the source didn't get deleted while we are processing.
                    var source = ContentLibrarySourceCache.Get( pair.SourceId );
                    if ( source != null )
                    {
                        var indexItem = ContentChannelItemDocument.LoadByModel( pair.Item, source );
                        IndexContainer.IndexDocument( indexItem );
                    }
                }
            }
        }

        /// <inheritdoc/>
        void IRockContentLibraryIndexable.DeleteAllContentLibraryDocuments( int? sourceId )
        {
            if ( sourceId.HasValue )
            {
                IndexContainer.DeleteDocumentByProperty( typeof( ContentChannelItemDocument ),
                    nameof( ContentChannelItemDocument.SourceId ),
                    sourceId );
            }
            else
            {
                IndexContainer.DeleteDocumentsByType<ContentChannelItemDocument>();
            }
        }

        #endregion IRockContentLibraryIndexable Methods
    }
}
