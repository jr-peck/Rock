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

using Rock.UniversalSearch.IndexModels.Attributes;
using Rock.Web.Cache;

namespace Rock.UniversalSearch.ContentLibraryDocuments
{
    /// <summary>
    /// Common base class for all content library item documents.
    /// </summary>
    internal abstract class DocumentBase : IndexModels.IndexModelBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier of the original entity this document represents.
        /// </summary>
        /// <value>
        /// The identifier of the original entity this document represents.
        /// </value>
        [RockIndexField( Index = IndexType.NotIndexed )]
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the display name of this document.
        /// </summary>
        /// <value>
        /// The display name of htis document.
        /// </value>
        [RockIndexField( Boost = 3 )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the common detail content of this document.
        /// </summary>
        /// <value>
        /// The common detail content of this document.
        /// </value>
        [RockIndexField]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the type name of the source item this document was
        /// built up from. This is namespace plus class name.
        /// </summary>
        /// <value>
        /// The type name of the source item this document was built up from.
        /// </value>
        [RockIndexField( Index = IndexType.NotIndexed )]
        public string ItemType { get; set; }

        /// <summary>
        /// Gets or sets the type name of the source entity this documentis
        /// associated with. This is namespace plus class name.
        /// </summary>
        /// <value>
        /// The type name of the source item this document was built up from.
        /// </value>
        [RockIndexField( Index = IndexType.NotIndexed )]
        public string SourceType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the library source this item belongs to.
        /// </summary>
        /// <value>
        /// The identifier of the library source this item belongs to.
        /// </value>
        [RockIndexField( Index = IndexType.NotIndexed )]
        public int SourceId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the library source this item belongs to.
        /// </summary>
        /// <value>
        /// The unique identifier of the library source this item belongs to.
        /// </value>
        /// <inheritdoc/>
        [RockIndexField( Index = IndexType.NotIndexed )]
        public Guid SourceGuid { get; set; }

        /// <summary>
        /// Gets or sets the identifier key of the library source this item belongs to.
        /// </summary>
        /// <value>
        /// The identifier key of the library source this item belongs to.
        /// </value>
        [RockIndexField( Index = IndexType.NotIndexed )]
        public string SourceIdKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this item is currently trending.
        /// </summary>
        /// <value>
        /// A value indicating if this item is currently trending.
        /// </value>
        [RockIndexField( Type = IndexFieldType.Boolean )]
        public bool IsTrending { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the current rank if this item is trending.
        /// </summary>
        /// <value>
        /// A value specifying the current rank if this item is trending.
        /// </value>
        [RockIndexField( Type = IndexFieldType.Number )]
        public int TrendingRank { get; set; }

        /// <summary>
        /// Gets or sets the personalization segment identifiers this item belongs to.
        /// </summary>
        /// <value>
        /// The personalization segment identifiers this item belongs to.
        /// </value>
        [RockIndexField( Type = IndexFieldType.Number )]
        public List<int> Segments { get; set; }

        // TODO
        // public string RequestFilters { get; set; }

        /// <summary>
        /// Gets or sets the primary date and time for this item. This should be the
        /// next upcoming effective date of the item.
        /// </summary>
        /// <value>
        /// The primary date and time for this item.
        /// </value>
        [RockIndexField( Type = IndexFieldType.Date )]
        public DateTime? RelevanceDateTime { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a document Id value that is comprised of both the modelId and
        /// the sourceId.
        /// </summary>
        /// <param name="modelId">The identifier of the model.</param>
        /// <param name="sourceId">The identifier of the source related to the model.</param>
        /// <returns>A signed long that represents the document identifier.</returns>
        protected static long GetDocumentId( int modelId, int sourceId )
        {
            // Store the modelId in the lower 32 bits and the sourceId in the
            // upper 32 bits of the long.
            ulong lowerValue = ( ulong ) modelId + int.MaxValue;
            ulong higherValue = ( ( ( ulong ) sourceId + int.MaxValue ) << 32 );

            // Forcibly cast the unsigned long to a signed long. This keeps
            // all the bits in the same place so we can later reverse this.
            return ( long ) ( lowerValue | higherValue );
        }

        /// <summary>
        /// Gets the model identifier from the document identifer that was previously
        /// created by <see cref="GetDocumentId(int, int)"/>.
        /// </summary>
        /// <param name="documentId">The unique identifier.</param>
        /// <returns>The model identifier from the document identifier.</returns>
        protected static int GetModelIdFromDocumentId( long documentId )
        {
            return ( int ) ( ( documentId & 0xFFFFFFFF ) - int.MaxValue );
        }

        /// <summary>
        /// Gets the source identifier from the document identifer that was previously
        /// created by <see cref="GetDocumentId(int, int)"/>.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>The source identifier from the document identifier.</returns>
        protected static int GetSourceIdFromUniqueId( long documentId )
        {
            return ( int ) ( ( documentId >> 32 ) - int.MaxValue );
        }

        /// <summary>
        /// Adds the indexable attributes to this index.
        /// </summary>
        /// <param name="sourceModel">The source model that has the attributes.</param>
        protected void AddIndexableAttributes( Attribute.IHasAttributes sourceModel )
        {
            sourceModel.LoadAttributes();

            foreach ( var attributeValue in sourceModel.AttributeValues )
            {
                // check that the attribute is marked as IsIndexEnabled
                var attribute = AttributeCache.Get( attributeValue.Value.AttributeId );

                if ( attribute.IsIndexEnabled )
                {
                    var key = MakeAttributeKeySafe( attributeValue.Key );

                    this[$"{key}ValueRaw"] = attributeValue.Value;
                    this[$"{key}ValueFormatted"] = attributeValue.Value.ValueFormatted;
                }
            }
        }

        /// <summary>
        /// Ensures the attribute key is safe to use in the index engine. It
        /// is possible for this to cause duplicate keys, but that is a slim
        /// chance and we are good with that. It would just overwrite one of
        /// the values so it's not a massive problem.
        /// </summary>
        /// <param name="key">The attribute key to be made safe.</param>
        /// <returns>A string that represents the attribute key safe for use in a search index.</returns>
        internal static string MakeAttributeKeySafe( string key )
        {
            // remove invalid characters
            key = key.Replace( ".", "_" );
            key = key.Replace( ",", "_" );
            key = key.Replace( "#", "_" );
            key = key.Replace( "*", "_" );
            key = key.StartsWith( "_" ) ? key.Substring( 1 ) : key;

            return key;
        }

        #endregion
    }
}
