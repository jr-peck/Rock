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

import { computed, defineComponent, PropType } from "vue";
import { ListItemBag } from "@Obsidian/ViewModels/Utility/listItemBag";
import { ContentLibraryFilterControl } from "@Obsidian/Enums/CMS/contentLibraryFilterControl";
import { AttributeFilterBag } from "@Obsidian/ViewModels/Blocks/CMS/ContentLibraryDetail/attributeFilterBag";
import { areEqual } from "@Obsidian/Utility/guid";
import { FieldType } from "@Obsidian/SystemGuids";
import SearchFilter from "./searchFilter";

export default defineComponent({
    name: "CMS.ContentLibraryDetail.AttributeSearchFilter",

    components: {
        SearchFilter
    },

    props: {
        /** The attribute filter to be displayed. */
        modelValue: {
            type: Object as PropType<AttributeFilterBag>,
            required: true
        }
    },
    
    setup(props) {
        const isEnabled = computed((): boolean => {
            return props.modelValue.isEnabled;
        });

        const isInconsistent = computed((): boolean => {
            return props.modelValue.isInconsistent;
        });

        const title = computed((): string => {
            return props.modelValue.attributeName ?? "";
        });

        const subtitle = computed((): string => {
            return `(${props.modelValue.fieldTypeName})`;
        });

        const description = computed((): string => {
            return `Sources Using: ${props.modelValue.sourceNames?.join(", ")}`;
        });

        const values = computed((): ListItemBag[] => {
            const values: ListItemBag[] = [
                {
                    text: "Filter Label",
                    value: props.modelValue.fieldTypeName
                }
            ];

            // Special handling for booleans. They only have a filter control
            // and its value is always Boolean.
            if (areEqual(props.modelValue.fieldTypeGuid, FieldType.Boolean)) {
                values.push({text: "Filter Control", value: "Boolean"});
            }
            else {
                values.push({
                    text: "Filter Control",
                    value: props.modelValue.filterControl === ContentLibraryFilterControl.Dropdown ? "Dropdown" : "Pills"
                });

                values.push({
                    text: "Filter Mode",
                    value: props.modelValue.isMultipleSelection ? "Multi-Select" : "Single-Select"
                });
            }

            return values;
        });

        return {
            description,
            isEnabled,
            isInconsistent,
            subtitle,
            title,
            values
        };
    },

    template: `
<SearchFilter :isEnabled="isEnabled"
    :isInconsistent="isInconsistent"
    :title="title"
    :subtitle="subtitle"
    :description="description"
    :values="values" />
`
});
