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

import { computed, defineComponent, PropType, ref } from "vue";
import Panel from "@Obsidian/Controls/panel";
import RockButton from "@Obsidian/Controls/rockButton";
import SectionHeader from "@Obsidian/Controls/sectionHeader";
import { ContentLibraryBag } from "@Obsidian/ViewModels/Blocks/CMS/ContentLibraryDetail/contentLibraryBag";
import { ListItemBag } from "@Obsidian/ViewModels/Utility/listItemBag";

export const SearchFilter = defineComponent({
    name: "CMS.ContentLibraryDetail.SearchFilter",

    components: {
        RockButton
    },

    props: {
        isEnabled: {
            type: Boolean as PropType<boolean>,
            default: false
        },
        
        isEditable: {
            type: Boolean as PropType<boolean>,
            default: false
        },

        title: {
            type: String as PropType<string>,
            required: true
        },

        description: {
            type: String as PropType<string>
        },

        values: {
            type: Array as PropType<ListItemBag[]>
        }
    },

    setup(props) {
        return {
        };
    },

    template: `
<div class="search-filter-row">
    <div class="search-filter-icon">
        <i v-if="isEnabled" class="fa fa-check-square" style="color: var(--brand-color);"></i>
        <i v-else class="fa fa-check-square-o" style="color: #c3c2c2;"></i>
    </div>

    <div class="search-filter-content">
        <div class="search-filter-title">{{ title }}</div>
        <div v-if="description" class="search-filter-description">{{ description }}</div>

        <fieldset>
            <dl v-for="value in values">
                <dt>{{ value.text }}</dt>
                <dd>{{ value.value }}</dd>
            </dl>
        </fieldset>
    </div>

    <div class="search-filter-actions">
        <RockButton v-if="isEditable" btnSize="sm"><i class="fa fa-pencil"></i></RockButton>
    </div>
</div>
`
});

export default defineComponent({
    name: "CMS.ContentLibraryDetail.SearchFilters",

    components: {
        Panel,
        RockButton,
        SearchFilter,
        SectionHeader
    },

    props: {
        /** The content library that contains the search filters to display and edit. */
        modelValue: {
            type: Object as PropType<ContentLibraryBag>,
            required: true
        }
    },

    setup(props) {
        // #region Values

        // #endregion

        // #region Computed Values

        const isFullTextSearchEnabled = computed((): boolean => {
            return false;
        });

        const isYearSearchEnabled = computed((): boolean => {
            return true;
        });

        const yearSearchValues = computed((): ListItemBag[] => {
            return [
                {
                    value: "Filter Label",
                    text: "Year"
                },
                {
                    value: "Filter Control",
                    text: "Pills"
                },
                {
                    value: "Filter Mode",
                    text: "Multi-Select"
                }
            ];
        });

        // #endregion

        // #region Functions

        // #endregion

        // #region Event Handlers

        // #endregion

        return {
            isFullTextSearchEnabled,
            isYearSearchEnabled,
            yearSearchValues
        };
    },

    template: `
<Panel title="Search Filters">
    <SectionHeader title="Search Filters" description="The configuration below allows you to set various ways your library can be filtered." />

    <SearchFilter :isEnabled="isFullTextSearchEnabled"
        isEditable
        title="Full Text Search"
        description="Uses the content field of the content channel item or description of an Event Item." />

        <SearchFilter :isEnabled="isYearSearchEnabled"
        isEditable
        title="Year"
        description="Uses the content channel item's start date to determine the year of the content."
        :values="yearSearchValues" />
</Panel>
`
//<i class="fa fa-check-square-o" style="color: gray;"></i>  <i style="color: var(--brand-color);" class="fa fa-check-square"></i>
});
