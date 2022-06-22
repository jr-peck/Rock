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
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.ViewModels.Blocks;
using Rock.ViewModels.Blocks.CMS.ContentLibraryDetail;
using Rock.ViewModels.Utility;
using Rock.Web.Cache;

namespace Rock.Blocks.CMS
{
    /// <summary>
    /// Displays the details of a particular content library.
    /// </summary>
    /// <seealso cref="Rock.Blocks.RockObsidianDetailBlockType" />

    [DisplayName( "Content Library Detail" )]
    [Category( "CMS" )]
    [Description( "Displays the details of a particular content library." )]
    [IconCssClass( "fa fa-book-open" )]

    #region Block Attributes

    #endregion

    public class ContentLibraryDetail : RockObsidianDetailBlockType
    {
        #region Keys

        private static class PageParameterKey
        {
            public const string ContentLibraryId = "ContentLibraryId";
        }

        private static class NavigationUrlKey
        {
            public const string ParentPage = "ParentPage";
        }

        #endregion Keys

        #region Methods

        /// <inheritdoc/>
        public override object GetObsidianBlockInitialization()
        {
            using ( var rockContext = new RockContext() )
            {
                var box = new DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag>();

                SetBoxInitialEntityState( box, true, rockContext );

                box.NavigationUrls = GetBoxNavigationUrls();
                box.Options = GetBoxOptions( box.IsEditable );
                box.QualifiedAttributeProperties = GetAttributeQualifiedColumns<ContentLibrary>();

                return box;
            }
        }

        /// <summary>
        /// Gets the box options required for the component to render the view
        /// or edit the entity.
        /// </summary>
        /// <param name="isEditable"><c>true</c> if the entity is editable; otherwise <c>false</c>.</param>
        /// <returns>The options that provide additional details to the block.</returns>
        private ContentLibraryDetailOptionsBag GetBoxOptions( bool isEditable )
        {
            var options = new ContentLibraryDetailOptionsBag();

            return options;
        }

        /// <summary>
        /// Validates the ContentLibrary for any final information that might not be
        /// valid after storing all the data from the client.
        /// </summary>
        /// <param name="contentLibrary">The ContentLibrary to be validated.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="errorMessage">On <c>false</c> return, contains the error message.</param>
        /// <returns><c>true</c> if the ContentLibrary is valid, <c>false</c> otherwise.</returns>
        private bool ValidateContentLibrary( ContentLibrary contentLibrary, RockContext rockContext, out string errorMessage )
        {
            errorMessage = null;

            if ( contentLibrary.LibraryKey.IsNullOrWhiteSpace() )
            {
                errorMessage = "Library Key is required.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the initial entity state of the box. Populates the Entity or
        /// ErrorMessage properties depending on the entity and permissions.
        /// </summary>
        /// <param name="box">The box to be populated.</param>
        /// <param name="loadAttributes"><c>true</c> if attributes and values should be loaded; otherwise <c>false</c>.</param>
        /// <param name="rockContext">The rock context.</param>
        private void SetBoxInitialEntityState( DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag> box, bool loadAttributes, RockContext rockContext )
        {
            var entity = GetInitialEntity( rockContext );

            if ( entity != null )
            {
                var isViewable = BlockCache.IsAuthorized( Security.Authorization.VIEW, RequestContext.CurrentPerson );
                box.IsEditable = BlockCache.IsAuthorized( Security.Authorization.EDIT, RequestContext.CurrentPerson );

                if ( loadAttributes )
                {
                    entity.LoadAttributes( rockContext );
                }

                if ( entity.Id != 0 )
                {
                    // Existing entity was found, prepare for view mode by default.
                    if ( isViewable )
                    {
                        box.Entity = GetEntityBagForView( entity, loadAttributes, rockContext );
                        box.SecurityGrantToken = GetSecurityGrantToken( entity );
                    }
                    else
                    {
                        box.ErrorMessage = EditModeMessage.NotAuthorizedToView( ContentLibrary.FriendlyTypeName );
                    }
                }
                else
                {
                    // New entity is being created, prepare for edit mode by default.
                    if ( box.IsEditable )
                    {
                        box.Entity = GetEntityBagForEdit( entity, loadAttributes );
                        box.SecurityGrantToken = GetSecurityGrantToken( entity );
                    }
                    else
                    {
                        box.ErrorMessage = EditModeMessage.NotAuthorizedToEdit( ContentLibrary.FriendlyTypeName );
                    }
                }
            }
            else
            {
                box.ErrorMessage = $"The {ContentLibrary.FriendlyTypeName} was not found.";
            }
        }

        /// <summary>
        /// Gets the entity bag that is common between both view and edit modes.
        /// </summary>
        /// <param name="entity">The entity to be represented as a bag.</param>
        /// <returns>A <see cref="ContentLibraryBag"/> that represents the entity.</returns>
        private ContentLibraryBag GetCommonEntityBag( ContentLibrary entity )
        {
            if ( entity == null )
            {
                return null;
            }

            return new ContentLibraryBag
            {
                IdKey = entity.IdKey,
                Description = entity.Description,
                EnableRequestFilters = entity.EnableRequestFilters,
                EnableSegments = entity.EnableSegments,
                FilterSettings = entity.FilterSettings,
                LastIndexDateTime = entity.LastIndexDateTime,
                LastIndexItemCount = entity.LastIndexItemCount,
                LibraryKey = entity.LibraryKey,
                Name = entity.Name,
                TrendingEnabled = entity.TrendingEnabled,
                TrendingGravity = entity.TrendingGravity,
                TrendingMaxItems = entity.TrendingMaxItems,
                TrendingWindowDay = entity.TrendingWindowDay
            };
        }

        /// <summary>
        /// Gets the bag for viewing the specied entity.
        /// </summary>
        /// <param name="entity">The entity to be represented for view purposes.</param>
        /// <param name="loadAttributes"><c>true</c> if attributes and values should be loaded; otherwise <c>false</c>.</param>
        /// <param name="rockContext">The context to use when accessing the database.</param>
        /// <returns>A <see cref="ContentLibraryBag"/> that represents the entity.</returns>
        private ContentLibraryBag GetEntityBagForView( ContentLibrary entity, bool loadAttributes, RockContext rockContext )
        {
            if ( entity == null )
            {
                return null;
            }

            var bag = GetCommonEntityBag( entity );

            if ( loadAttributes )
            {
                bag.LoadAttributesAndValuesForPublicView( entity, RequestContext.CurrentPerson );
            }

            bag.Sources = entity.ContentLibrarySources
                .Select( s => GetContentSourceBag( s, rockContext ) )
                .Where( s => s != null )
                .ToList();

            return bag;
        }

        /// <summary>
        /// Gets the bag for editing the specied entity.
        /// </summary>
        /// <param name="entity">The entity to be represented for edit purposes.</param>
        /// <param name="loadAttributes"><c>true</c> if attributes and values should be loaded; otherwise <c>false</c>.</param>
        /// <returns>A <see cref="ContentLibraryBag"/> that represents the entity.</returns>
        private ContentLibraryBag GetEntityBagForEdit( ContentLibrary entity, bool loadAttributes )
        {
            if ( entity == null )
            {
                return null;
            }

            var bag = GetCommonEntityBag( entity );

            if ( loadAttributes )
            {
                bag.LoadAttributesAndValuesForPublicEdit( entity, RequestContext.CurrentPerson );
            }

            return bag;
        }

        /// <summary>
        /// Updates the entity from the data in the save box.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <param name="box">The box containing the information to be updated.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns><c>true</c> if the box was valid and the entity was updated, <c>false</c> otherwise.</returns>
        private bool UpdateEntityFromBox( ContentLibrary entity, DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag> box, RockContext rockContext )
        {
            if ( box.ValidProperties == null )
            {
                return false;
            }

            var contentLibraryService = new ContentLibraryService( rockContext );

            box.IfValidProperty( nameof( box.Entity.Description ),
                () => entity.Description = box.Entity.Description );

            box.IfValidProperty( nameof( box.Entity.EnableRequestFilters ),
                () => entity.EnableRequestFilters = box.Entity.EnableRequestFilters );

            box.IfValidProperty( nameof( box.Entity.EnableSegments ),
                () => entity.EnableSegments = box.Entity.EnableSegments );

            box.IfValidProperty( nameof( box.Entity.FilterSettings ),
                () => entity.FilterSettings = box.Entity.FilterSettings );

            box.IfValidProperty( nameof( box.Entity.LibraryKey ), () =>
            {
                var libraryId = entity.Id != 0 ? ( int? ) entity.Id : null;
                entity.LibraryKey = contentLibraryService.GetUniqueSlug( box.Entity.LibraryKey, libraryId );
            } );

            box.IfValidProperty( nameof( box.Entity.Name ),
                () => entity.Name = box.Entity.Name );

            box.IfValidProperty( nameof( box.Entity.TrendingEnabled ),
                () => entity.TrendingEnabled = box.Entity.TrendingEnabled );

            box.IfValidProperty( nameof( box.Entity.TrendingMaxItems ),
                () => entity.TrendingMaxItems = box.Entity.TrendingMaxItems );

            box.IfValidProperty( nameof( box.Entity.TrendingWindowDay ),
                () => entity.TrendingWindowDay = box.Entity.TrendingWindowDay );

            box.IfValidProperty( nameof( box.Entity.TrendingGravity ),
                () => entity.TrendingGravity = box.Entity.TrendingGravity );

            box.IfValidProperty( nameof( box.Entity.AttributeValues ),
                () =>
                {
                    entity.LoadAttributes( rockContext );

                    entity.SetPublicAttributeValues( box.Entity.AttributeValues, RequestContext.CurrentPerson );
                } );

            return true;
        }

        /// <summary>
        /// Gets the initial entity from page parameters or creates a new entity
        /// if page parameters requested creation.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <returns>The <see cref="ContentLibrary"/> to be viewed or edited on the page.</returns>
        private ContentLibrary GetInitialEntity( RockContext rockContext )
        {
            return GetInitialEntity<ContentLibrary, ContentLibraryService>( rockContext, PageParameterKey.ContentLibraryId );
        }

        /// <summary>
        /// Gets the box navigation URLs required for the page to operate.
        /// </summary>
        /// <returns>A dictionary of key names and URL values.</returns>
        private Dictionary<string, string> GetBoxNavigationUrls()
        {
            return new Dictionary<string, string>
            {
                [NavigationUrlKey.ParentPage] = this.GetParentPageUrl()
            };
        }

        /// <inheritdoc/>
        protected override string RenewSecurityGrantToken()
        {
            using ( var rockContext = new RockContext() )
            {
                var entity = GetInitialEntity( rockContext );

                if ( entity != null )
                {
                    entity.LoadAttributes( rockContext );
                }

                return GetSecurityGrantToken( entity );
            }
        }

        /// <summary>
        /// Gets the security grant token that will be used by UI controls on
        /// this block to ensure they have the proper permissions.
        /// </summary>
        /// <param name="entity">The entity being viewed or edited on this block.</param>
        /// <returns>A string that represents the security grant token.</string>
        private string GetSecurityGrantToken( IHasAttributes entity )
        {
            return new Rock.Security.SecurityGrant()
                .AddRulesForAttributes( entity, RequestContext.CurrentPerson )
                .ToToken();
        }

        /// <summary>
        /// Attempts to load an entity to be used for an edit action.
        /// </summary>
        /// <param name="idKey">The identifier key of the entity to load.</param>
        /// <param name="rockContext">The database context to load the entity from.</param>
        /// <param name="entity">Contains the entity that was loaded when <c>true</c> is returned.</param>
        /// <param name="error">Contains the action error result when <c>false</c> is returned.</param>
        /// <returns><c>true</c> if the entity was loaded and passed security checks.</returns>
        private bool TryGetEntityForEditAction( string idKey, RockContext rockContext, out ContentLibrary entity, out BlockActionResult error, Func<IQueryable<ContentLibrary>, IQueryable<ContentLibrary>> qryAdditions = null )
        {
            var entityService = new ContentLibraryService( rockContext );
            IQueryable<ContentLibrary> qry;
            error = null;

            // Determine if we are editing an existing entity or creating a new one.
            if ( idKey.IsNotNullOrWhiteSpace() )
            {
                // If editing an existing entity then load it and make sure it
                // was found and can still be edited.
                qry = entityService.GetQueryableByKey( idKey, !PageCache.Layout.Site.DisablePredictableIds );

                if ( qryAdditions != null )
                {
                    qry = qryAdditions( qry );
                }

                entity = qry.SingleOrDefault();
            }
            else
            {
                // Create a new entity.
                entity = new ContentLibrary();
                entityService.Add( entity );
            }

            if ( entity == null )
            {
                error = ActionBadRequest( $"{ContentLibrary.FriendlyTypeName} not found." );
                return false;
            }

            if ( !BlockCache.IsAuthorized( Security.Authorization.EDIT, RequestContext.CurrentPerson ) )
            {
                error = ActionBadRequest( $"Not authorized to edit ${ContentLibrary.FriendlyTypeName}." );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Translates the <paramref name="source"/> into a bag that can be
        /// sent to the client to display and edit the source.
        /// </summary>
        /// <param name="source">The library source that will be sent to the client.</param>
        /// <param name="rockContext">The context to use when accessing the database.</param>
        /// <returns>A new <see cref="ContentSourceBag"/> instance that represents <paramref name="source"/>.</returns>
        private static ContentSourceBag GetContentSourceBag( ContentLibrarySource source, RockContext rockContext )
        {
            var contentChannelEntityTypeId = EntityTypeCache.GetId<ContentChannel>() ?? 0;
            var eventCalendarEntityTypeId = EntityTypeCache.GetId<EventCalendar>() ?? 0;
            string name;
            Guid entityGuid;
            string color;
            string iconCssClass;
            int itemCount;

            // Process the entity as a content channel source.
            if ( source.EntityTypeId == contentChannelEntityTypeId )
            {
                var contentChannel = new ContentChannelService( rockContext ).Get( source.EntityId );

                if ( contentChannel == null )
                {
                    return null;
                }

                name = contentChannel.Name;
                entityGuid = contentChannel.Guid;
                itemCount = new ContentChannelItemService( rockContext ).Queryable()
                    .Where( cci => cci.ContentChannelId == contentChannel.Id )
                    .Count();
                color = "#009ce3";
                iconCssClass = contentChannel.IconCssClass.ToStringOrDefault( "fa fa-bullhorn" );
            }

            // Process the entity as an event calendar source.
            else if ( source.EntityTypeId == eventCalendarEntityTypeId )
            {
                var eventCalendar = new EventCalendarService( rockContext ).Get( source.EntityId );

                if ( eventCalendar == null )
                {
                    return null;
                }

                name = eventCalendar.Name;
                entityGuid = eventCalendar.Guid;
                itemCount = new EventCalendarItemService( rockContext ).Queryable()
                    .Where( cci => cci.EventCalendarId == eventCalendar.Id )
                    .Count();
                color = "#09ae77";
                iconCssClass = eventCalendar.IconCssClass.ToStringOrDefault( "fa fa-calendar-alt" );
            }
            else
            {
                return null;
            }

            // Get the additional settings or a default object.
            var additionalSettings = source.AdditionalSettings
                    .FromJsonOrNull<AdditionalSourceSettings>() ?? new AdditionalSourceSettings();

            // Create the bag that represents the source.
            return new ContentSourceBag
            {
                Guid = source.Guid,
                Name = name,
                EntityTypeGuid = EntityTypeCache.Get( source.EntityTypeId ).Guid,
                EntityGuid = entityGuid,
                Color = color,
                IconCssClass = iconCssClass,
                OccurrencesToShow = source.OccurrencesToShow,
                ItemCount = itemCount,
                Attributes = additionalSettings.AttributeGuids
                    .Select( g => AttributeCache.Get( g ) )
                    .Where( g => g != null )
                    .Select( g => new ListItemBag
                    {
                        Value = g.Guid.ToString(),
                        Text = g.Name
                    } )
                    .ToList()
            };
        }

        #endregion

        #region Block Actions

        /// <summary>
        /// Gets the box that will contain all the information needed to begin
        /// the edit operation.
        /// </summary>
        /// <param name="key">The identifier of the entity to be edited.</param>
        /// <returns>A box that contains the entity and any other information required.</returns>
        [BlockAction]
        public BlockActionResult Edit( string key )
        {
            using ( var rockContext = new RockContext() )
            {
                if ( !TryGetEntityForEditAction( key, rockContext, out var entity, out var actionError ) )
                {
                    return actionError;
                }

                entity.LoadAttributes( rockContext );

                var box = new DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag>
                {
                    Entity = GetEntityBagForEdit( entity, true )
                };

                return ActionOk( box );
            }
        }

        /// <summary>
        /// Saves the entity contained in the box.
        /// </summary>
        /// <param name="box">The box that contains all the information required to save.</param>
        /// <returns>A new entity bag to be used when returning to view mode, or the URL to redirect to after creating a new entity.</returns>
        [BlockAction]
        public BlockActionResult Save( DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag> box )
        {
            using ( var rockContext = new RockContext() )
            {
                var entityService = new ContentLibraryService( rockContext );

                if ( !TryGetEntityForEditAction( box.Entity.IdKey, rockContext, out var entity, out var actionError ) )
                {
                    return actionError;
                }

                // Update the entity instance from the information in the bag.
                if ( !UpdateEntityFromBox( entity, box, rockContext ) )
                {
                    return ActionBadRequest( "Invalid data." );
                }

                // Ensure everything is valid before saving.
                if ( !ValidateContentLibrary( entity, rockContext, out var validationMessage ) )
                {
                    return ActionBadRequest( validationMessage );
                }

                var isNew = entity.Id == 0;

                rockContext.WrapTransaction( () =>
                {
                    rockContext.SaveChanges();
                    entity.SaveAttributeValues( rockContext );
                } );

                if ( isNew )
                {
                    return ActionContent( System.Net.HttpStatusCode.Created, this.GetCurrentPageUrl( new Dictionary<string, string>
                    {
                        [PageParameterKey.ContentLibraryId] = entity.IdKey
                    } ) );
                }

                // Ensure navigation properties will work now.
                entity = entityService.Get( entity.Id );
                entity.LoadAttributes( rockContext );

                return ActionOk( GetEntityBagForView( entity, true, rockContext ) );
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="key">The identifier of the entity to be deleted.</param>
        /// <returns>A string that contains the URL to be redirected to on success.</returns>
        [BlockAction]
        public BlockActionResult Delete( string key )
        {
            using ( var rockContext = new RockContext() )
            {
                var entityService = new ContentLibraryService( rockContext );

                if ( !TryGetEntityForEditAction( key, rockContext, out var entity, out var actionError ) )
                {
                    return actionError;
                }

                if ( !entityService.CanDelete( entity, out var errorMessage ) )
                {
                    return ActionBadRequest( errorMessage );
                }

                entityService.Delete( entity );
                rockContext.SaveChanges();

                return ActionOk( this.GetParentPageUrl() );
            }
        }

        /// <summary>
        /// Refreshes the list of attributes that can be displayed for editing
        /// purposes based on any modified values on the entity.
        /// </summary>
        /// <param name="box">The box that contains all the information about the entity being edited.</param>
        /// <returns>A box that contains the entity and attribute information.</returns>
        [BlockAction]
        public BlockActionResult RefreshAttributes( DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag> box )
        {
            using ( var rockContext = new RockContext() )
            {
                if ( !TryGetEntityForEditAction( box.Entity.IdKey, rockContext, out var entity, out var actionError ) )
                {
                    return actionError;
                }

                // Update the entity instance from the information in the bag.
                if ( !UpdateEntityFromBox( entity, box, rockContext ) )
                {
                    return ActionBadRequest( "Invalid data." );
                }

                // Reload attributes based on the new property values.
                entity.LoadAttributes( rockContext );

                var refreshedBox = new DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag>
                {
                    Entity = GetEntityBagForEdit( entity, true )
                };

                var oldAttributeGuids = box.Entity.Attributes.Values.Select( a => a.AttributeGuid ).ToList();
                var newAttributeGuids = refreshedBox.Entity.Attributes.Values.Select( a => a.AttributeGuid );

                // If the attributes haven't changed then return a 204 status code.
                if ( oldAttributeGuids.SequenceEqual( newAttributeGuids ) )
                {
                    return ActionStatusCode( System.Net.HttpStatusCode.NoContent );
                }

                // Replace any values for attributes that haven't changed with
                // the value sent by the client. This ensures any unsaved attribute
                // value changes are not lost.
                foreach ( var kvp in refreshedBox.Entity.Attributes )
                {
                    if ( oldAttributeGuids.Contains( kvp.Value.AttributeGuid ) )
                    {
                        refreshedBox.Entity.AttributeValues[kvp.Key] = box.Entity.AttributeValues[kvp.Key];
                    }
                }

                return ActionOk( refreshedBox );
            }
        }

        /// <summary>
        /// Gets a list of all the content channels that are available for the
        /// individual to use when adding or editing a content channel source.
        /// </summary>
        /// <returns>A collection of <see cref="AvailableContentSourceBag"/> objects.</returns>
        [BlockAction]
        public BlockActionResult GetAvailableContentChannels()
        {
            using ( var rockContext = new RockContext() )
            {
                var contentChannels = new ContentChannelService( rockContext )
                    .Queryable()
                    .OrderBy( c => c.Name )
                    .ToList()
                    .Where( c => c.IsAuthorized( Authorization.VIEW, RequestContext.CurrentPerson ) )
                    .ToList();

                var availableSources = contentChannels
                    .Select( c =>
                    {
                        var item = new ContentChannelItem
                        {
                            ContentChannelId = c.Id
                        };

                        item.LoadAttributes( rockContext );

                        return new AvailableContentSourceBag
                        {
                            Guid = c.Guid,
                            Name = c.Name,
                            Attributes = item.Attributes.Select( a => new ListItemBag
                            {
                                Value = a.Value.Guid.ToString(),
                                Text = a.Value.Name,
                                Category = a.Value.FieldType.Name
                            } ).ToList()
                        };
                    } );

                return ActionOk( availableSources );
            }
        }

        /// <summary>
        /// Gets a list of all the event calendars that are available for the
        /// individual to use when adding or editing a event calendar source.
        /// </summary>
        /// <returns>A collection of <see cref="AvailableContentSourceBag"/> objects.</returns>
        [BlockAction]
        public BlockActionResult GetAvailableEventCalendars()
        {
            using ( var rockContext = new RockContext() )
            {
                var eventCalendars = new EventCalendarService( rockContext )
                    .Queryable()
                    .OrderBy( ec => ec.Name )
                    .ToList()
                    .Where( ec => ec.IsAuthorized( Authorization.VIEW, RequestContext.CurrentPerson ) )
                    .ToList();

                var availableSources = eventCalendars
                    .Select( ec =>
                    {
                        var item = new EventCalendarItem
                        {
                            EventCalendarId = ec.Id
                        };

                        item.LoadAttributes( rockContext );

                        return new AvailableContentSourceBag
                        {
                            Guid = ec.Guid,
                            Name = ec.Name,
                            Attributes = item.Attributes.Select( a => new ListItemBag
                            {
                                Value = a.Value.Guid.ToString(),
                                Text = a.Value.Name,
                                Category = a.Value.FieldType.Name
                            } ).ToList()
                        };
                    } );

                return ActionOk( availableSources );
            }
        }

        /// <summary>
        /// Saves the edits to a library source. This will either update an
        /// existing library source or create a new one.
        /// </summary>
        /// <param name="key">The identifier of the content library to be modified.</param>
        /// <param name="bag">The source to be added or updated.</param>
        /// <returns>The detail box that contains the new entity information to be displayed.</returns>
        [BlockAction]
        public BlockActionResult SaveLibrarySource( string key, ContentSourceBag bag )
        {
            using ( var rockContext = new RockContext() )
            {
                var contentLibraryService = new ContentLibraryService( rockContext );

                if ( !TryGetEntityForEditAction( key, rockContext, out var library, out var actionError, qry => qry.Include( l => l.ContentLibrarySources ) ) )
                {
                    return actionError;
                }

                int entityTypeId = 0;
                int entityId = 0;

                // Find the source entity to be added. Either a content channel
                // or an event calendar.
                if ( bag.EntityTypeGuid == SystemGuid.EntityType.CONTENT_CHANNEL.AsGuid() )
                {
                    // Verify the content channel specified exists and that the
                    // person has access to it.
                    var contentChannel = new ContentChannelService( rockContext ).Get( bag.EntityGuid );

                    if ( contentChannel == null )
                    {
                        return ActionNotFound( "Content channel was not found." );
                    }

                    if ( !contentChannel.IsAuthorized( Authorization.VIEW, RequestContext.CurrentPerson ) )
                    {
                        return ActionForbidden( "Not authorized to view this content channel." );
                    }

                    entityTypeId = EntityTypeCache.GetId<ContentChannel>().Value;
                    entityId = contentChannel.Id;
                }
                else if ( bag.EntityTypeGuid == SystemGuid.EntityType.EVENT_CALENDAR.AsGuid() )
                {
                    // Verify the event calendar specified exists and that the
                    // person has access to it.
                    var eventCalendar = new EventCalendarService( rockContext ).Get( bag.EntityGuid );

                    if ( eventCalendar == null )
                    {
                        return ActionNotFound( "Event calendar was not found." );
                    }

                    if ( !eventCalendar.IsAuthorized( Authorization.VIEW, RequestContext.CurrentPerson ) )
                    {
                        return ActionForbidden( "Not authorized to view this event calendar." );
                    }

                    entityTypeId = EntityTypeCache.GetId<EventCalendar>().Value;
                    entityId = eventCalendar.Id;
                }
                else
                {
                    return ActionBadRequest( "Invalid source type." );
                }

                // Find the existing matching source or create a new one.
                var source = library.ContentLibrarySources
                    .Where( s => s.EntityTypeId == entityTypeId && s.EntityId == entityId )
                    .FirstOrDefault();

                if ( source == null )
                {
                    source = new ContentLibrarySource
                    {
                        EntityTypeId = entityTypeId,
                        EntityId = entityId
                    };
                    library.ContentLibrarySources.Add( source );
                }

                // Update the source with the new settings.
                var additionalSettings = new AdditionalSourceSettings
                {
                    AttributeGuids = bag.Attributes?.Select( a => a.Value.AsGuid() ).ToList() ?? new List<Guid>()
                };

                source.OccurrencesToShow = bag.OccurrencesToShow;
                source.AdditionalSettings = additionalSettings.ToJson();

                rockContext.SaveChanges();

                // Load the attributes on the library and return the new entity bag.
                library.LoadAttributes( rockContext );

                var box = new DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag>
                {
                    Entity = GetEntityBagForView( library, true, rockContext )
                };

                return ActionOk( box );
            }
        }

        /// <summary>
        /// Deletes an existing library source from the content library.
        /// </summary>
        /// <param name="key">The identifier of the content library to be modified.</param>
        /// <param name="sourceGuid">The unique identifier of the source to be deleted.</param>
        /// <returns>The detail box that contains the new entity information to be displayed.</returns>
        [BlockAction]
        public BlockActionResult DeleteLibrarySource( string key, Guid sourceGuid )
        {
            using ( var rockContext = new RockContext() )
            {
                var contentLibraryService = new ContentLibraryService( rockContext );
                var contentLibrarySourceService = new ContentLibrarySourceService( rockContext );

                if ( !TryGetEntityForEditAction( key, rockContext, out var library, out var actionError, qry => qry.Include( l => l.ContentLibrarySources ) ) )
                {
                    return actionError;
                }

                // Find the existing matching source or create a new one.
                var source = library.ContentLibrarySources
                    .Where( s => s.Guid == sourceGuid )
                    .SingleOrDefault();

                if ( source == null )
                {
                    return ActionNotFound( "Source was not found." );
                }

                // Delete the source.
                contentLibrarySourceService.Delete( source );
                rockContext.SaveChanges();

                // Send back a new box with the entity to display.
                library.LoadAttributes( rockContext );

                var box = new DetailBlockBox<ContentLibraryBag, ContentLibraryDetailOptionsBag>
                {
                    Entity = GetEntityBagForView( library, true, rockContext )
                };

                return ActionOk( box );
            }
        }

        #endregion

        /// <summary>
        /// The structured object for the additional settings of a content
        /// library source.
        /// </summary>
        private class AdditionalSourceSettings
        {
            /// <summary>
            /// Gets or sets the attribute unique identifiers that are enabled
            /// for the content source.
            /// </summary>
            /// <value>
            /// The attribute unique identifiers that are enabled.
            /// </value>
            public List<Guid> AttributeGuids { get; set; }
        }
    }
}
